using Models.Base;

namespace Models.Dtos.UserRoom
{
    public class UserRoomDto : BaseModel
    {
        public int UserId { get; set; }
        public int RoomId { get; set; }
    }
}