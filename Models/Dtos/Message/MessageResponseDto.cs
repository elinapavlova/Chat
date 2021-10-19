using Models.Base;

namespace Models.Dtos.Message
{
    public class MessageResponseDto : BaseModel
    {
        public string Text { get; set; }
        public int UserId { get; set; }
        public int RoomId { get; set; }
    }
}