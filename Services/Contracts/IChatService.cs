using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Filter;
using Infrastructure.Result;
using Models.Dtos.Chat;

namespace Services.Contracts
{
    public interface IChatService
    {
        Task<ResultContainer<ChatDto>> Create(ChatDto chat);
        Task<ResultContainer<ICollection<ChatDto>>> GetByName(string title, int page, int pageSize);
        Task<ResultContainer<ChatDto>> GetById(int id);
        Task<ResultContainer<ChatResponseDto>> GetByIdWithMessages(int id, int page, int pageSize);
        Task<ResultContainer<int?>> CountChatsByRoomId(int roomId);
    }
}