using System;
using Models.Base;

namespace Models
{
    public class UserChat : BaseModel
    {
        public int UserId { get; set; }
        public int ChatId { get; set; }
        
        public DateTime DateComeOut { get; set; }
        
        public User User { get; set; }
        public Chat Chat { get; set; }
    }
}