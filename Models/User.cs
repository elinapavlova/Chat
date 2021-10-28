using System.Collections.Generic;
using Models.Base;

namespace Models
{
    public class User : BaseModel
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public List<Room> Rooms { get; set; }
        public List<Chat> Chats { get; set; }
        public List<Message> Messages { get; set; }
    }
}