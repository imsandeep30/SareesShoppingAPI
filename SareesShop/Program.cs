using Microsoft.EntityFrameworkCore;
using SareesShop.Data;
using SareesShop.Services;
using Npgsql.EntityFrameworkCore.PostgreSQL; // Add this using directive

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DbContext (PostgreSQL)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                       ?? Environment.GetEnvironmentVariable("DATABASE_URL");

builder.Services.AddDbContext<AppDbContext>(options =>
{
    // If DATABASE_URL exists (Render), convert it
    if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("DATABASE_URL")))
    {
        var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL")!;
        var databaseUri = new Uri(databaseUrl);

        var userInfo = databaseUri.UserInfo.Split(':');
        var pgConnectionString = $"Host={databaseUri.Host};Port={databaseUri.Port};Database={databaseUri.AbsolutePath.TrimStart('/')};Username={userInfo[0]};Password={userInfo[1]};SSL Mode=Require;Trust Server Certificate=true";

        options.UseNpgsql(pgConnectionString);
    }
    else
    {
        // fallback to appsettings.json
        options.UseNpgsql(connectionString);
    }
});



// Cloudinary Service
builder.Services.AddScoped<CloudnaryService>();

// CORS (for frontend React)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod());
});

var app = builder.Build();

// Automatically apply migrations
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate(); // Ensures tables are created if missing
}

// Use Swagger only in development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();
