using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Result;
using Models.Dtos.Chat;
using Models.Dtos.User;
using Models.Dtos.UserChat;

namespace Services.Contracts
{
    public interface IUserChatService
    {
        Task<ResultContainer<UserChatDto>> Create(UserChatDto userChatDto);
        Task<ResultContainer<ICollection<ChatDto>>> GetChatsUserIn(int userId, int page, int pageSize);
        Task<ResultContainer<ICollection<UserDto>>> GetUsersByChatId(int chatId);
        Task<ResultContainer<UserChatResponseDto>> ComeOutOfChat(int userId, int chatId);
        Task<ResultContainer<UserChatDto>> CheckUserInChat(int userId, int chatId);
    }
}