using System.Collections.Generic;
using Models.Base;

namespace Models
{
    public class User : BaseModel
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public List<Room> RoomsCreated { get; set; }
        public List<Chat> ChatsCreated { get; set; }
        
        public List<UserRoom> RoomsIn { get; set; }
        public List<UserChat> ChatsIn { get; set; }
        
        public List<Message> Messages { get; set; }
    }
}