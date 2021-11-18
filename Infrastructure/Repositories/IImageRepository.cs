using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public interface IImageRepository
    {
        Task DeleteImageByPath(string path);
    }
}