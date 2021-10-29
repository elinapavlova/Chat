using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Contracts.Base;
using Infrastructure.Filter;
using Models;

namespace Infrastructure.Contracts
{
    public interface IUserChatRepository : IBaseRepository<UserChat, BaseFilter>
    {
        Task<List<Chat>> GetChatsByUserId(int userId, int page, int pageSize);
        Task<List<User>> GetUsersInChat(int chatId);
        Task<UserChat> CheckUserInChat(int userId, int chatId);
        Task<UserChat> ComeOutOfChat(int userId, int chatId);
    }
}