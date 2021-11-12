using System.Collections.Generic;
using Models.Base;
using Models.Dtos.File;
using Models.Dtos.Image;

namespace Models.Dtos.Message
{
    public class MessageResponseDto : BaseModel
    {
        public string Text { get; set; }
        public int UserId { get; set; }
        public int ChatId { get; set; }
        
        public ICollection<ImageResponseDto> Images { get; set; }
        public ICollection<FileResponseDto> Unuploaded { get; set; }
    }
}