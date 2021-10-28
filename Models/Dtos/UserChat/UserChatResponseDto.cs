using System;
using Models.Base;

namespace Models.Dtos.UserChat
{
    public class UserChatResponseDto : BaseModel
    {
        public int UserId { get; set; }
        public int ChatId { get; set; }
        public DateTime? DateComeOut { get; set; }
    }
}