using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Contracts.Base;
using Infrastructure.Filter;
using Models;

namespace Infrastructure.Contracts
{
    public interface IChatRepository : IBaseRepository<Chat>
    {
        Task<Chat> GetByIdWithMessages(int id, BaseFilterDto filter);
        Task<ICollection<Chat>> GetByName(string name, BaseFilterDto filter);
        Task<ICollection<Chat>> GetByRoomId(int roomId, BaseFilterDto filter);
        Task<int?> Count(int roomId);
    }
}