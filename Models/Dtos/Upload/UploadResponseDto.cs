using System.Collections.Generic;
using Models.Dtos.File;

namespace Models.Dtos.Upload
{
    public class UploadResponseDto
    {
        public IList<FileResponseDto> Data { get; set; }
        public int? ErrorType { get; set; }
    }
}