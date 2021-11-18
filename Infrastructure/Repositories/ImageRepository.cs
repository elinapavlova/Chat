using System.Threading.Tasks;
using Database;
using Microsoft.EntityFrameworkCore;

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
    }
}