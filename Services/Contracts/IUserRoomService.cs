using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Result;
using Models.Dtos.Room;
using Models.Dtos.User;
using Models.Dtos.UserRoom;

namespace Services.Contracts
{
    public interface IUserRoomService
    {
        Task<ResultContainer<UserRoomDto>> CreateUserRoomAsync(UserRoomDto userRoomDto);
        Task<ResultContainer<ICollection<RoomDto>>> GetRoomsUserIn(int userId, int page, int pageSize);
        Task<ResultContainer<ICollection<UserDto>>> GetUsersInRoom(int roomId);
        Task<ResultContainer<UserRoomResponseDto>> ComeOutOfRoom(int userId, int roomId);
        Task<ResultContainer<UserRoomDto>> CheckUserInRoom(int userId, int roomId);
    }
}