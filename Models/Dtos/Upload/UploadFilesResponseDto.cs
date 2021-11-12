using System.Collections.Generic;
using Models.Dtos.File;
using Models.Dtos.Image;

namespace Models.Dtos.Upload
{
    public class UploadFilesResponseDto
    {
        public List<ImageResponseDto> Images { get; set; }
    }
}