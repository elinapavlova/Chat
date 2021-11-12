using Microsoft.EntityFrameworkCore;
using Models;

namespace Database
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<UserRoom> UsersRooms { get; set; }
        public DbSet<UserChat> UsersChats { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            builder.Entity<Room>(room =>
            {
                room.HasOne(r => r.User)
                    .WithMany(u => u.RoomsCreated)
                    .HasForeignKey(r => r.UserId);

                room.Property(r => r.Title).IsRequired().HasMaxLength(200);
            });
            
            builder.Entity<Message>(message =>
            {
                message.HasOne(m => m.Chat)
                    .WithMany(r => r.Messages)
                    .HasForeignKey(m => m.ChatId);

                message.HasOne(m => m.User)
                    .WithMany(u => u.Messages)
                    .HasForeignKey(m => m.UserId);
                
                message.Property(m => m.Text).HasMaxLength(500);
            });

            builder.Entity<User>(user =>
            {
                user.Property(u => u.Email).IsRequired().HasMaxLength(50);
                user.Property(u => u.Password).IsRequired().HasMaxLength(200);
            });

            builder.Entity<Image>(image =>
            {
                image.HasOne(i => i.Message)
                    .WithMany(m => m.Images)
                    .HasForeignKey(i => i.MessageId);

                image.Property(i => i.Path).IsRequired().HasMaxLength(200);
                image.Property(i => i.Name).IsRequired().HasMaxLength(100);
            });
            
            builder.Entity<Chat>(chat =>
            {
                chat.HasOne(c => c.User)
                    .WithMany(u => u.ChatsCreated)
                    .HasForeignKey(c => c.UserId);
                chat.HasOne(c => c.Room)
                    .WithMany(r => r.Chats)
                    .HasForeignKey(c => c.RoomId);

                chat.Property(r => r.Title).IsRequired().HasMaxLength(200);
            });

            builder.Entity<UserRoom>(userRoom =>
            {
                userRoom.HasOne(ur => ur.User)
                    .WithMany(u => u.RoomsIn)
                    .HasForeignKey(ur => ur.UserId);
                
                userRoom.HasOne(ur => ur.Room)
                    .WithMany(r => r.UsersRooms)
                    .HasForeignKey(ur => ur.RoomId);
            });
            
            builder.Entity<UserChat>(userChat =>
            {
                userChat.HasOne(ur => ur.User)
                    .WithMany(u => u.ChatsIn)
                    .HasForeignKey(ur => ur.UserId);
                
                userChat.HasOne(ur => ur.Chat)
                    .WithMany(r => r.UsersChats)
                    .HasForeignKey(ur => ur.ChatId);
            });
        }
    }
}