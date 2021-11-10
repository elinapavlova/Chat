using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Filter;
using Infrastructure.Result;
using Models.Dtos.Chat;
using Models.Dtos.User;
using Models.Dtos.UserChat;

namespace Services.Contracts
{
    public interface IUserChatService
    {
        Task<ResultContainer<UserChatDto>> Create(UserChatDto userChatDto);
        Task<ResultContainer<ICollection<ChatDto>>> GetChatsUserIn(int userId, FilterPagingDto filter);
        Task<ResultContainer<ICollection<UserDto>>> GetUsersByChatId(int chatId, FilterPagingDto filter);
        Task<ResultContainer<UserChatResponseDto>> ComeOutOfChat(int userId, int chatId);
        Task<ResultContainer<UserChatDto>> CheckUserInChat(int userId, int chatId);
    }
}