using System.Threading.Tasks;
using Infrastructure.Result;
using Microsoft.AspNetCore.Http;
using Models.Dtos;
using Models.Dtos.Upload;

namespace Services.Contracts
{
    public interface IFileStorageService
    {
        Task<ResultContainer<UploadFilesResponseDto>> Upload(IFormFileCollection files, int messageId);
    }
}