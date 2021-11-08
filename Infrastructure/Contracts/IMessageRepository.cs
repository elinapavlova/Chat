using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Contracts.Base;
using Infrastructure.Filter;
using Models;

namespace Infrastructure.Contracts
{
    public interface IMessageRepository : IBaseRepository<Message>
    {
        Task<ICollection<Message>> GetByChatIdAsync(int chatId, BaseFilter filter);
        Task<int?> Count(int chatId);
    }
}