using System.Collections.Generic;
using Models.FileModel;

namespace Models.UploadModel
{
    public class UploadRequestDto
    {
        public int MessageId { get; set; }
        public ICollection<FileRequestDto> Files { get; set; }
    }
}