using Models.Base;

namespace Models.Dtos.UserChat
{
    public class UserChatDto : BaseModel
    {
        public int UserId { get; set; }
        public int ChatId { get; set; }
    }
}