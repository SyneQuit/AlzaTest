using Data.Configurations;
using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Repositories
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
        }

        public DbSet<Product> Products => Set<Product>();
    }
}
