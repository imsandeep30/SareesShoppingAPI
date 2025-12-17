using Microsoft.EntityFrameworkCore;
using SareesShop.Data;
using SareesShop.Services;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// PostgreSQL DbContext (LOCAL + RENDER)
builder.Services.AddDbContext<AppDbContext>(options =>
{
    var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
    Console.WriteLine("DATABASE_URL: " + databaseUrl);

    if (!string.IsNullOrEmpty(databaseUrl))
    {
        // Parse DATABASE_URL safely
        var uri = new Uri(databaseUrl);
        var userInfo = uri.UserInfo.Split(':');
        var host = uri.Host;
        var port = uri.Port == -1 ? 5432 : uri.Port; // default port
        var dbName = uri.AbsolutePath.TrimStart('/');
        var username = userInfo[0];
        var password = userInfo[1];

        var connectionString = $"Host={host};Port={port};Database={dbName};Username={username};Password={password};SSL Mode=Require;Trust Server Certificate=true";

        options.UseNpgsql(connectionString);
    }
    else
    {
        // Local database
        options.UseNpgsql(
            builder.Configuration.GetConnectionString("DefaultConnection")
        );
    }
});


// Cloudinary
builder.Services.AddScoped<CloudnaryService>();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod());
});

var app = builder.Build();

// Apply migrations automatically
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

// Swagger only in Development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();
app.Run();
