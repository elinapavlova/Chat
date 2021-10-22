using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Filter;
using Models;

namespace Infrastructure.Contracts
{
    public interface IImageRepository  : IBaseRepository<Image, BaseFilter>
    {
        Task<ICollection<Image>> GetByMessageId(int messageId);
    }
}