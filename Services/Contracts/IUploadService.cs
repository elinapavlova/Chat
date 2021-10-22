using System.Threading.Tasks;
using Infrastructure.Result;
using Microsoft.AspNetCore.Http;
using Models.Dtos;
using Models.Dtos.Message;

namespace Services.Contracts
{
    public interface IUploadService
    {
        Task<ResultContainer<UploadResponseDto>> UploadAsync(IFormFileCollection files, int messageId);
    }
}