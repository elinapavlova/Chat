using Microsoft.EntityFrameworkCore;
using Models;

namespace Database
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Message> Messages { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            builder.Entity<Room>(room =>
            {
                room.HasOne(r => r.User)
                    .WithMany(u => u.Rooms)
                    .HasForeignKey(r => r.IdUser);
            });
            
            builder.Entity<Message>(message =>
            {
                message.HasOne(m => m.Room)
                    .WithMany(r => r.Messages)
                    .HasForeignKey(m => m.IdRoom);

                message.HasOne(m => m.User)
                    .WithMany(u => u.Messages)
                    .HasForeignKey(m => m.IdUser);
            });
        }
    }
}