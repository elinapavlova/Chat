using Models.Base;

namespace Models
{
    public class Message : BaseModel
    {
        public string Text { get; set; }
        public int IdUser { get; set; }
        public int IdRoom { get; set; }
        
        public User User { get; set; }
        public Room Room { get; set; }
    }
}