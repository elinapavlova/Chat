using Models.Base;

namespace Models
{
    public class Image : BaseModel
    {
        public byte[] Img { get; set; }
        public int MessageId { get; set; }
        
        public Message Message { get; set; }
    }
}