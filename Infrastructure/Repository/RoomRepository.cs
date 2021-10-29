using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database;
using Infrastructure.Contracts;
using Infrastructure.Filter;
using Infrastructure.Repository.Base;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Infrastructure.Repository
{
    public class RoomRepository : BaseRepository<Room, BaseFilter>, IRoomRepository
    {
        private readonly AppDbContext _context;
        
        public RoomRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Room> GetByIdWithChatsAsync(int id, int page, int pageSize)
        {
            var room = await _context.Rooms.SingleOrDefaultAsync(u => u.Id == id);

            if (room == null)
                return null;
            
            var chats = await _context.Chats
                .Where(m => m.RoomId == room.Id)
                .OrderByDescending(r => r.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            room.Chats = chats;
            return room;
        }

        public async Task<ICollection<Room>> FindByNameAsync(string title, int page, int pageSize)
        {
            var rooms = await _context.Rooms
                .Where(r => r.Title.Contains(title))
                .OrderByDescending(r => r.DateCreated)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return rooms;
        }
    }
}