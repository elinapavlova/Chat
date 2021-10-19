using Database;
using Infrastructure.Contracts;
using Infrastructure.Filter;
using Infrastructure.Repository.Base;
using Models;

namespace Infrastructure.Repository
{
    public class ImageRepository : BaseRepository<Image, BaseFilter>, IImageRepository
    {
        public ImageRepository(AppDbContext context) : base(context)
        {
        }
    }
}