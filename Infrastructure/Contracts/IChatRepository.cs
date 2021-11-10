using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Contracts.Base;
using Infrastructure.Filter;
using Models;

namespace Infrastructure.Contracts
{
    public interface IChatRepository : IBaseRepository<Chat>
    {
        Task<Chat> GetByIdWithMessages(int id, int page, int pageSize);
        Task<ICollection<Chat>> GetByName(string name, int page, int pageSize);
        Task<ICollection<Chat>> GetByRoomId(int roomId, int page, int pageSize);
        Task<int?> Count(int roomId);
    }
}