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

        public async Task<Room> GetByIdWithMessagesAsync(int id, int page, int pageSize)
        {
            var room = await _context.Rooms
                .SingleOrDefaultAsync(u => u.Id == id);

            if (room == null)
                return null;
            
            var messages = await _context.Messages
                .Where(m => m.IdRoom == room.Id)
                .OrderByDescending(r => r.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            room.Messages = messages;
            return room;
        }

        public async Task<ICollection<Room>> FindByNameAsync(string title)
        {
            var rooms = await _context.Rooms
                .Where(r => r.Title.Contains(title))
                .OrderByDescending(r => r.Date)
                .ToListAsync();
            return rooms;
        }
    }
}