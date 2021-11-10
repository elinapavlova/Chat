using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Contracts.Base;
using Models;

namespace Infrastructure.Contracts
{
    public interface IRoomRepository : IBaseRepository<Room>
    {
        Task<Room> GetByIdWithChats(int id, int page, int pageSize);
        Task<ICollection<Room>> GetByName(string name, int page, int pageSize);
        Task<ICollection<Room>> GetFiltered(int page, int pageSize, string columnName, bool isDescending);
    }
}