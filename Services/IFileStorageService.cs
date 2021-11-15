using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Result;
using Models.FileModel;
using Models.UploadModel;

namespace Services
{
    public interface IFileStorageService
    {
        Task<ResultContainer<ICollection<FileResponseDto>>> Upload(UploadRequestDto files);
    }
}