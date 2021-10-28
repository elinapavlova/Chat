using System.Collections.Generic;
using Models.Base;

namespace Models
{
    public class Chat : BaseModel
    {
        public string Title { get; set; }
        public int RoomId { get; set; }
        public int UserId { get; set; }
        
        public Room Room { get; set; }
        public User User { get; set; }
        public ICollection<Message> Messages { get; set; }
    }
}