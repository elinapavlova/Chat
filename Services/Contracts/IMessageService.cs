using System.Threading.Tasks;
using Infrastructure.Result;
using Models.Dtos.Message;

namespace Services.Contracts
{
    public interface IMessageService
    {
        Task<ResultContainer<MessageResponseDto>> Create(MessageRequestDto message);
        Task<ResultContainer<MessageResponseDto>> GetById(int id);
        Task<ResultContainer<int?>> CountMessagesByChatId(int chatId);
    }
}