using Database;
using Infrastructure.Contracts;
using Infrastructure.Filter;
using Infrastructure.Repository.Base;
using Models;

namespace Infrastructure.Repository
{
    public class MessageRepository : BaseRepository<Message, BaseFilter>, IMessageRepository
    {
        public MessageRepository(AppDbContext context) : base(context)
        {
        }
    }
}