using System.Collections.Generic;
using Models.Dtos.Image;

namespace Models.Dtos
{
    public class UploadResponseDto
    {
        public List<ImageResponseDto> Images { get; set; }
    }
}