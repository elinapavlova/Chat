using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Result;
using Microsoft.AspNetCore.Http;
using Models.FileModel;

namespace Services
{
    public interface IFileStorageService
    {
        Task<ResultContainer<ICollection<FileResponseDto>>> Upload(IFormFileCollection files);
    }
}