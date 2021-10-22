using Models.Base;

namespace Models.Dtos
{
    public class ImageResponseDto : BaseModel
    {
        public byte[] Img { get; set; }
    }
}