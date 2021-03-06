using Models.Base;

namespace Models
{
    public class Image : BaseModel
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public int MessageId { get; set; }
        
        public Message Message { get; set; }
    }
}