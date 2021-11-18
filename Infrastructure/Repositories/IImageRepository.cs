using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public interface IImageRepository
    {
        Task DeleteImageByPath(string path);
        Task<List<string>> GetPathsForNotExistingFiles();
    }
}