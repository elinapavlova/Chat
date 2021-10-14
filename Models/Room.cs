using System.Collections.Generic;
using Models.Base;

namespace Models
{
    public class Room : BaseModel
    {
        public string Title { get; set; }
        public int IdUser { get; set; }
        
        public User User { get; set; }
        public List<Message> Messages { get; set; }
    }
}