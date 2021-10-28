using System.Collections.Generic;
using Models.Base;
using Models.Dtos.Chat;

namespace Models.Dtos.Room
{
    public class RoomResponseDto : BaseModel
    {
        public string Title { get; set; }
        public int UserId { get; set; }

        public ICollection<ChatResponseDto> Chats { get; set; }
    }
}