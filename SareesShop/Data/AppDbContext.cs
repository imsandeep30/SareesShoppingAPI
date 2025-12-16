using Microsoft.EntityFrameworkCore;
using SareesShop.Models;
namespace SareesShop.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Saree> Sarees => Set<Saree>();
    }
}
