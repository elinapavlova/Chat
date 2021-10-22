using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database;
using Infrastructure.Contracts;
using Infrastructure.Filter;
using Infrastructure.Repository.Base;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Infrastructure.Repository
{
    public class ImageRepository : BaseRepository<Image, BaseFilter>, IImageRepository
    {
        private readonly AppDbContext _context;
        public ImageRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<ICollection<Image>> GetByMessageId(int messageId)
        {
            var result = await _context.Images
                .Where(i => i.MessageId == messageId)
                .ToListAsync();
            return result;
        }
    }
}