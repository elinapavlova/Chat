using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Contracts.Base;
using Infrastructure.Filter;
using Models;

namespace Infrastructure.Contracts
{
    public interface IUserRoomRepository : IBaseRepository<UserRoom, BaseFilter>
    {
        Task<List<Room>> GetRoomsByUserId(int userId, int page, int pageSize);
        Task<List<User>> GetUsersInRoom(int roomId);
        Task<UserRoom> CheckUserInRoom(int userId, int roomId);
        Task<UserRoom> ComeOutOfRoom(int userId, int roomId);
    }
}