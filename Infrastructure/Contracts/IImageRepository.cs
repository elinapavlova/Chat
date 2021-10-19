using Infrastructure.Filter;
using Models;

namespace Infrastructure.Contracts
{
    public interface IImageRepository  : IBaseRepository<Image, BaseFilter>
    {
    }
}