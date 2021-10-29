using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Contracts.Base;
using Infrastructure.Filter;
using Models;

namespace Infrastructure.Contracts
{
    public interface IChatRepository : IBaseRepository<Chat, BaseFilter>
    {
        Task<Chat> GetByIdWithMessagesAsync(int id, int page, int pageSize);
        Task<ICollection<Chat>> FindByNameAsync(string name);
    }
}