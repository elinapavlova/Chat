using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Contracts.Base;
using Models;

namespace Infrastructure.Contracts
{
    public interface IImageRepository  : IBaseRepository<Image>
    {
        Task<ICollection<Image>> GetByMessageId(int messageId);
    }
}