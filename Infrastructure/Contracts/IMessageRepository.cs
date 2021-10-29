using Infrastructure.Contracts.Base;
using Infrastructure.Filter;
using Models;

namespace Infrastructure.Contracts
{
    public interface IMessageRepository : IBaseRepository<Message, BaseFilter>
    {
    }
}