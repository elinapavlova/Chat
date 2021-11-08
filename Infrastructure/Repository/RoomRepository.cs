using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database;
using Infrastructure.Contracts;
using Infrastructure.Filter;
using Infrastructure.Options;
using Infrastructure.Repository.Base;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Infrastructure.Repository
{
    public class RoomRepository : BaseRepository<Room, BaseFilter>, IRoomRepository
    {
        private readonly AppDbContext _context;
        private readonly IChatRepository _chatRepository;
        
        public RoomRepository(AppDbContext context, PagingOptions options, IChatRepository chatRepository) 
            : base(context, options)
        {
            _context = context;
            _chatRepository = chatRepository;
        }

        public async Task<Room> GetByIdWithChatsAsync(int id, BaseFilterDto filter)
        {
            var room = await _context.Rooms.FirstOrDefaultAsync(u => u.Id == id);
            if (room == null)
                return null;

            var chats = await _chatRepository.GetByRoomIdAsync(id, filter);
            if (chats.Count == 0)
                return room;

            room.Chats = chats;
            return room;
        }

        public async Task<ICollection<Room>> FindByNameAsync(string title, BaseFilterDto filter)
        {
            var rooms = _context.Rooms.Where(r => r.Title.Contains(title));
            var filteredRooms = await GetFilteredSource(rooms, filter);
            
            return filteredRooms;
        }
        
        public async Task<ICollection<Room>> GetFiltered(BaseFilterDto filter)
        {
            var result = _context.Set<Room>().AsNoTracking();

            result = ApplySort(result, filter.Sort);
            result = ApplyPaging(result, filter.Paging);

            return await result.ToListAsync();
        }
    }
}