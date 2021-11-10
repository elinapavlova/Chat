using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Contracts.Base;
using Infrastructure.Filter;
using Models;

namespace Infrastructure.Contracts
{
    public interface IUserRoomRepository : IBaseRepository<UserRoom>
    {
        Task<List<Room>> GetRoomsByUserId(int userId, int page, int pageSize);
        Task<List<User>> GetUsersByRoomId(int roomId, int page, int pageSize);
        Task<UserRoom> CheckUserInRoom(int userId, int roomId);
        Task<UserRoom> ComeOutOfRoom(int userId, int roomId);
    }
}