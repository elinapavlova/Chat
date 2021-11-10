using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Filter;
using Infrastructure.Result;
using Models.Dtos.Room;
using Models.Dtos.User;
using Models.Dtos.UserRoom;

namespace Services.Contracts
{
    public interface IUserRoomService
    {
        Task<ResultContainer<UserRoomDto>> Create(UserRoomDto userRoomDto);
        Task<ResultContainer<ICollection<RoomDto>>> GetRoomsUserIn(int userId, FilterPagingDto filter);
        Task<ResultContainer<ICollection<UserDto>>> GetUsersByRoomId(int roomId, FilterPagingDto filter);
        Task<ResultContainer<UserRoomResponseDto>> ComeOutOfRoom(int userId, int roomId);
        Task<ResultContainer<UserRoomDto>> CheckUserInRoom(int userId, int roomId);
    }
}