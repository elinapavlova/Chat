using Microsoft.EntityFrameworkCore;
using Models;
using Models.ImageModel;

namespace Database
{
    public class AppDbContext : DbContext
    {
        public DbSet<Image> Images { get; set; }
        
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            builder.Entity<Image>(image =>
            {
                image.Property(i => i.Path).IsRequired();
                image.Property(i => i.Name).IsRequired().HasMaxLength(100);
            });
        }
    }
}