using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Filter;
using Models;

namespace Infrastructure.Contracts
{
    public interface IRoomRepository : IBaseRepository<Room, BaseFilter>
    {
        Task<Room> GetByIdWithMessagesAsync(int id, int page, int pageSize);
        Task<ICollection<Room>> FindByNameAsync(string name);
    }
}