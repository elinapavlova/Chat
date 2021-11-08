using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Contracts.Base;
using Infrastructure.Filter;
using Models;

namespace Infrastructure.Contracts
{
    public interface IUserRoomRepository : IBaseRepository<UserRoom>
    {
        Task<List<Room>> GetRoomsByUserId(int userId, BaseFilterDto filter);
        Task<List<User>> GetUsersInRoom(int roomId);
        Task<UserRoom> CheckUserInRoom(int userId, int roomId);
        Task<UserRoom> ComeOutOfRoom(int userId, int roomId);
    }
}