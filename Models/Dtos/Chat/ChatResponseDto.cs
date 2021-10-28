using System.Collections.Generic;
using Models.Base;
using Models.Dtos.Message;

namespace Models.Dtos.Chat
{
    public class ChatResponseDto : BaseModel
    {
        public string Title { get; set; }
        public int RoomId { get; set; }
        public int UserId { get; set; }

        public ICollection<MessageResponseDto> Messages { get; set; }
    }
}