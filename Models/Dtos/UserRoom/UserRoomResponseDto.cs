using System;
using Models.Base;

namespace Models.Dtos.UserRoom
{
    public class UserRoomResponseDto : BaseModel
    {
        public int UserId { get; set; }
        public int RoomId { get; set; }
        public DateTime? DateComeOut { get; set; }
    }
}