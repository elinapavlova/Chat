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
        
        public RoomRepository
        (
            AppDbContext context, 
            PagingOptions options, 
            IChatRepository chatRepository
        ) 
            : base(context, options)
        {
            _context = context;
            _chatRepository = chatRepository;
        }

        public async Task<Room> GetByIdWithChats(int id, int page, int pageSize)
        {
            var room = await _context.Rooms.FirstOrDefaultAsync(u => u.Id == id);
            if (room == null)
                return null;

            var filter = new BaseFilter
            {
                Paging = new FilterPagingDto {PageNumber = page, PageSize = pageSize}
            };
            
            var chats = await _chatRepository.GetByRoomId(id, filter.Paging.PageNumber, filter.Paging.PageSize);
            
            if (chats.Count == 0)
                return room;

            room.Chats = chats;
            return room;
        }

        public async Task<ICollection<Room>> GetByName(string title, int page, int pageSize)
        {
            var rooms = _context.Rooms.Where(r => r.Title.Contains(title));

            var filter = new BaseFilter
            {
                Paging = new FilterPagingDto {PageNumber = page, PageSize = pageSize}
            };
            
            var filteredRooms = await GetFilteredSource(rooms, filter);
            
            return filteredRooms;
        }
        
        public async Task<ICollection<Room>> GetFiltered(int page, int pageSize, string columnName, bool isDescending)
        {
            var filter = new BaseFilter
            {
                Paging = new FilterPagingDto {PageNumber = page, PageSize = pageSize},
                Sort = new FilterSortDto {ColumnName = columnName, IsDescending = isDescending}
            };
            
            var result = await GetFilteredSource(filter);

            return result;
        }
    }
}