using Microsoft.EntityFrameworkCore;
using Models;

namespace Database
{
    public class AppDbContext : DbContext
    {
        public DbSet<Image> Images { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}
    }
}