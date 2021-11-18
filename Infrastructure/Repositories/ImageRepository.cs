using System.Collections.Generic;
using System.Threading.Tasks;
using Database;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Infrastructure.Repositories
{
    public class ImageRepository : IImageRepository
    {
        private readonly AppDbContext _context;

        public ImageRepository
        (
            AppDbContext context
        )
        {
            _context = context;
        }

        public async Task DeleteImageByPath(string path)
        {
            var image = await _context.Images.FirstOrDefaultAsync(i => i.Path == path);
            _context.Images.Remove(image);
            await _context.SaveChangesAsync();
        }

        public async Task<List<string>> GetPathsForNotExistingFiles()
        {
            var paths = _context.Images
                .Select(i => i.Path)
                .AsEnumerable()
                .Where(path => !System.IO.File.Exists(path))
                .ToList();

            return paths;
        }
    }
}