using System.Threading.Tasks;
using Infrastructure.Result;
using Models.Dtos;

namespace Services.Contracts
{
    public interface IMessageService
    {
        Task<ResultContainer<MessageResponseDto>> CreateMessageAsync(MessageResponseDto message);
        Task<ResultContainer<MessageResponseDto>> FindByIdAsync(int id);
    }
}