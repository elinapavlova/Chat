using System.Collections.Generic;
using Models.Base;

namespace Models
{
    public class Message : BaseModel
    {
        public string Text { get; set; }
        public int UserId { get; set; }
        public int ChatId { get; set; }
        
        public User User { get; set; }
        
        public Chat Chat { get; set; }
        public List<Image> Images { get; set; }
    }
}