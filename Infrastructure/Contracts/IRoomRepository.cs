using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Contracts.Base;
using Infrastructure.Filter;
using Models;

namespace Infrastructure.Contracts
{
    public interface IRoomRepository : IBaseRepository<Room>
    {
        Task<Room> GetByIdWithChats(int id, BaseFilterDto filter);
        Task<ICollection<Room>> GetByName(string name, BaseFilterDto filter);
        Task<ICollection<Room>> GetFiltered(BaseFilterDto filter);
    }
}