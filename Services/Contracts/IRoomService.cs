using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Filter;
using Infrastructure.Result;
using Models.Dtos.Room;

namespace Services.Contracts
{
    public interface IRoomService
    {
        Task<ResultContainer<RoomDto>> Create(RoomDto room);
        Task<ResultContainer<ICollection<RoomDto>>> GetByName(string title, FilterPagingDto filter);
        Task<ResultContainer<ICollection<RoomDto>>> GetFiltered(FilterPagingDto paging, FilterSortDto sort);
        Task<ResultContainer<RoomDto>> GetById(int id);
        Task<ResultContainer<RoomResponseDto>> GetByIdWithChats(int id, FilterPagingDto filter);
    }
}