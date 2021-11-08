using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Result;
using Models.Dtos.Room;

namespace Services.Contracts
{
    public interface IRoomService
    {
        Task<ResultContainer<RoomDto>> Create(RoomDto room);
        Task<ResultContainer<ICollection<RoomDto>>> GetByName(string title, int page, int pageSize);
        Task<ResultContainer<ICollection<RoomDto>>> GetPage(int page, int pageSize, string columnName, bool isDescending);
        Task<ResultContainer<RoomDto>> GetById(int id);
        Task<ResultContainer<RoomResponseDto>> GetByIdWithChats(int id, int page, int pageSize);
    }
}