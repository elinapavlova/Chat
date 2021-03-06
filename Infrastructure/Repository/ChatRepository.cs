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
    public class ChatRepository : BaseRepository<Chat, BaseFilter>, IChatRepository
    {
        private readonly AppDbContext _context;
        private readonly IMessageRepository _messageRepository;
        
        public ChatRepository
        (
            AppDbContext context, 
            PagingOptions options, 
            IMessageRepository messageRepository
        ) 
            : base(context, options)
        {
            _context = context;
            _messageRepository = messageRepository;
        }
        
        public async Task<Chat> GetByIdWithMessages(int id, int page, int pageSize)
        {
            var chat = await _context.Chats.FirstOrDefaultAsync(u => u.Id == id);
            if (chat == null)
                return null;

            var filter = new BaseFilter
            {
                Paging = new FilterPagingDto {PageNumber = page, PageSize = pageSize}
            };
            
            var messages = await _messageRepository.GetByChatId(id, filter);
            if (messages.Count == 0)
                return chat;

            chat.Messages = messages;
            return chat;
        }

        public async Task<ICollection<Chat>> GetByName(string title, int page, int pageSize)
        {
            var chats = _context.Chats.Where(r => r.Title.Contains(title));
            
            var filter = new BaseFilter
            {
                Paging = new FilterPagingDto {PageNumber = page, PageSize = pageSize}
            };
            
            var filteredChats = await GetFilteredSource(chats, filter);
            
            return filteredChats;
        }

        public async Task<ICollection<Chat>> GetByRoomId(int roomId, int page, int pageSize)
        {
            var chats = _context.Chats.Where(r => r.RoomId == roomId);

            var filter = new BaseFilter
            {
                Paging = new FilterPagingDto{PageNumber = page, PageSize = pageSize}
            };
            
            var filteredChats = await GetFilteredSource(chats, filter);
            
            return filteredChats;
        }
        
        public async Task<int?> Count(int roomId)
        {
            var room = await _context.Rooms.FirstOrDefaultAsync(r => r.Id == roomId);
            if (room == null)
                return null;
            
            var chatsInRoom = await _context.Chats
                .Where(c => c.RoomId == roomId)
                .ToListAsync();
            
            var count = chatsInRoom.Count;
            
            return count;
        }
    }
}