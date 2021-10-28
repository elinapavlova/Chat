using Infrastructure.Filter;
using Models;

namespace Infrastructure.Contracts
{
    public interface IUserChatRepository : IBaseRepository<UserChat, BaseFilter>
    {
        
    }
}