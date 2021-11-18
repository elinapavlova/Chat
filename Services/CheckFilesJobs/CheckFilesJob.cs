using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Database;
using Infrastructure.Repositories;

namespace Services.CheckFilesJobs
{
    public class CheckFilesJob : ICheckFilesJob
    {
        private readonly AppDbContext _context;
        private readonly IImageRepository _imageRepository;

        public CheckFilesJob
        (
            AppDbContext context,
            IImageRepository imageRepository
        )
        {
            _context = context;
            _imageRepository = imageRepository;
        }
        
        public async Task DeleteImageFromDbIfFileNotExists()
        {
            var paths = _context.Images
                .Select(i => i.Path)
                .AsEnumerable()
                .Where(path => !File.Exists(path))
                .ToList();

            foreach (var path in paths) 
                await DeleteImageFromDb(path);
        }

        private async Task DeleteImageFromDb(string path)
        {
            await _imageRepository.DeleteImageByPath(path);
        }
    }
}