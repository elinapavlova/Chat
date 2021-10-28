using System;
using Models.Base;

namespace Models
{
    public class UserRoom : BaseModel
    {
        public int UserId { get; set; }
        public int RoomId { get; set; }
        
        public DateTime? DateComeOut { get; set; }
        
        public User User { get; set; }
        public Room Room { get; set; }
    }
}