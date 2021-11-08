using System.Threading.Tasks;
using Infrastructure.Result;
using Models.Dtos.Message;

namespace Services.Contracts
{
    public interface IMessageService
    {
        Task<ResultContainer<MessageResponseDto>> CreateMessageAsync(MessageRequestDto message);
        Task<ResultContainer<MessageResponseDto>> FindByIdAsync(int id);
        Task<ResultContainer<int?>> CountMessagesInChatAsync(int chatId);
    }
}