using Microsoft.AspNetCore.Http;

namespace Models.Dtos.Message
{
    public class MessageRequestDto
    {
        public string Text { get; set; }
        public int UserId { get; set; }
        public int ChatId { get; set; }
        
        public IFormFileCollection Files { get; set; }
    }
}