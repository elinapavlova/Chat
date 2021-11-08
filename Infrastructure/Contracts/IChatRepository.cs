using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Contracts.Base;
using Infrastructure.Filter;
using Models;

namespace Infrastructure.Contracts
{
    public interface IChatRepository : IBaseRepository<Chat>
    {
        Task<Chat> GetByIdWithMessagesAsync(int id, BaseFilterDto filter);
        Task<ICollection<Chat>> FindByNameAsync(string name, BaseFilterDto filter);
        Task<ICollection<Chat>> GetByRoomIdAsync(int roomId, BaseFilterDto filter);
        Task<int?> Count(int roomId);
    }
}