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
    public class MessageRepository : BaseRepository<Message, BaseFilter>, IMessageRepository
    {
        private readonly AppDbContext _context;
        public MessageRepository(AppDbContext context, PagingOptions options) : base(context, options)
        {
            _context = context;
        }

        public async Task<ICollection<Message>> GetByChatIdAsync(int chatId, BaseFilter filter)
        {
            var messagesInChat = _context.Messages
                .Where(m => m.ChatId == chatId);

            var filteredMessages = ApplySort(messagesInChat,filter.Sort);
            filteredMessages = ApplyPaging(filteredMessages, filter.Paging);
            
            var messages = await filteredMessages.ToListAsync();
            if (messages.Count == 0)
                return messages;
            
            foreach (var message in messages)
                message.Images = await GetImagesForMessage(message.Id);
            
            return messages;
        }
        
        private async Task<List<Image>> GetImagesForMessage(int messageId)
        {
            var images = await _context.Images
                .Where(i => i.MessageId == messageId)
                .OrderByDescending(i => i.Id)
                .ToListAsync();
            return images;
        }

        public async Task<int?> Count(int chatId)
        {
            var chat = await _context.Chats.FirstOrDefaultAsync(r => r.Id == chatId);
            if (chat == null)
                return null;
            
            var messagesInChat = await _context.Messages.Where(c => c.ChatId == chatId).ToListAsync();
            var count = messagesInChat.Count;
            return count;
        }
    }
}