using Models.Base;

namespace Models.Dtos.Image
{
    public class ImageResponseDto : BaseModel
    {
        public byte[] Img { get; set; }
    }
}