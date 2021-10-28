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
    public class ChatRepository : BaseRepository<Chat, BaseFilter>, IChatRepository
    {
        private readonly AppDbContext _context;
        
        public ChatRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        
        public async Task<Chat> GetByIdWithMessagesAsync(int id, int page, int pageSize)
        {
            var chat = await _context.Chats.SingleOrDefaultAsync(u => u.Id == id);

            if (chat == null)
                return null;
            
            var messages = await _context.Messages
                .Where(m => m.ChatId == chat.Id)
                .OrderByDescending(r => r.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            chat.Messages = messages;
            return chat;
        }

        public async Task<ICollection<Chat>> FindByNameAsync(string title)
        {
            var chats = await _context.Chats
                .Where(r => r.Title.Contains(title))
                .OrderByDescending(r => r.DateCreated)
                .ToListAsync();
            return chats;
        }
    }
}