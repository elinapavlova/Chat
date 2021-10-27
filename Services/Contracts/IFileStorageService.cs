using System.Threading.Tasks;
using Infrastructure.Result;
using Microsoft.AspNetCore.Http;
using Models.Dtos;

namespace Services.Contracts
{
    public interface IFileStorageService
    {
        Task<ResultContainer<UploadResponseDto>> UploadAsync(IFormFileCollection files, int messageId);
    }
}