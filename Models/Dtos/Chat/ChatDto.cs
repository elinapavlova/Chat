using Models.Base;

namespace Models.Dtos.Chat
{
    public class ChatDto : BaseModel
    {
        public string Title { get; set; }
        public int RoomId { get; set; }
        public int UserId { get; set; }
    }
}