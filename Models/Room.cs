using System.Collections.Generic;
using Models.Base;

namespace Models
{
    public class Room : BaseModel
    {
        public string Title { get; set; }
        public int UserId { get; set; }
        
        public User User { get; set; }
        public ICollection<Chat> Chats { get; set; }
        
        public List<UserRoom> UsersRooms { get; set; }
    }
}