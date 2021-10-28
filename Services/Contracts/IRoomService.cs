using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Result;
using Models.Dtos.Room;

namespace Services.Contracts
{
    public interface IRoomService
    {
        Task<ResultContainer<RoomDto>> CreateRoomAsync(RoomDto room);
        Task<ResultContainer<ICollection<RoomDto>>> FindByNameAsync(string title);
        Task<ResultContainer<ICollection<RoomDto>>> GetPageAsync(int page, int pageSize, string columnName, bool isDescending);
        Task<ResultContainer<RoomDto>> FindByIdAsync(int id);
        Task<ResultContainer<RoomResponseDto>> GetByIdWithChatsAsync(int id, int page, int pageSize);
    }
}