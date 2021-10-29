using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Contracts.Base;
using Infrastructure.Filter;
using Models;

namespace Infrastructure.Contracts
{
    public interface IRoomRepository : IBaseRepository<Room, BaseFilter>
    {
        Task<Room> GetByIdWithChatsAsync(int id, int page, int pageSize);
        Task<ICollection<Room>> FindByNameAsync(string name);
    }
}