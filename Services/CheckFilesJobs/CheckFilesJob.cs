using System.Threading.Tasks;
using Infrastructure.Repositories;

namespace Services.CheckFilesJobs
{
    public class CheckFilesJob : ICheckFilesJob
    {
        private readonly IImageRepository _imageRepository;

        public CheckFilesJob
        (
            IImageRepository imageRepository
        )
        {
            _imageRepository = imageRepository;
        }
        
        public async Task DeleteImageFromDbIfFileNotExists()
        {
            var paths = await _imageRepository.GetPathsForNotExistingFiles();

            foreach (var path in paths) 
                await DeleteImageFromDb(path);
        }

        private async Task DeleteImageFromDb(string path)
        {
            await _imageRepository.DeleteImageByPath(path);
        }
    }
}