using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Contracts.Base;
using Infrastructure.Filter;
using Models;

namespace Infrastructure.Contracts
{
    public interface IRoomRepository : IBaseRepository<Room>
    {
        Task<Room> GetByIdWithChatsAsync(int id, BaseFilterDto filter);
        Task<ICollection<Room>> FindByNameAsync(string name, BaseFilterDto filter);
        Task<ICollection<Room>> GetFiltered(BaseFilterDto filter);
    }
}