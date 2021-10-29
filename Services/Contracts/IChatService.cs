using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Result;
using Models.Dtos.Chat;

namespace Services.Contracts
{
    public interface IChatService
    {
        Task<ResultContainer<ChatDto>> CreateChatAsync(ChatDto chat);
        Task<ResultContainer<ICollection<ChatDto>>> FindByNameAsync(string title, int page, int pageSize);
        Task<ResultContainer<ChatDto>> FindByIdAsync(int id);
        Task<ResultContainer<ChatResponseDto>> GetByIdWithMessagesAsync(int id, int page, int pageSize);
    }
}