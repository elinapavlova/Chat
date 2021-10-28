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
        Task<ResultContainer<UserChatDto>> CreateUserChatAsync(UserChatDto userChatDto);
        Task<ResultContainer<ICollection<ChatDto>>> GetChatsUserIn(int userId);
        Task<ResultContainer<ICollection<UserDto>>> GetUsersInChat(int chatId);
        Task<ResultContainer<UserChatResponseDto>> ComeOutOfChat(int userId, int chatId);
        Task<ResultContainer<UserChatDto>> CheckUserInChat(int userId, int chatId);
    }
}