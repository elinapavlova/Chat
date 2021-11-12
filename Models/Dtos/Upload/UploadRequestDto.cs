using System.Collections.Generic;
using Models.Dtos.File;

namespace Models.Dtos.Upload
{
    public class UploadRequestDto
    {
        public int MessageId { get; set; }
        public ICollection<FileRequestDto> Files { get; init; }
    }
}