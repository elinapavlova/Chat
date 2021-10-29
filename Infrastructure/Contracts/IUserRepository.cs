using System.Threading.Tasks;
using Infrastructure.Contracts.Base;
using Infrastructure.Filter;
using Models;

namespace Infrastructure.Contracts
{
    public interface IUserRepository : IBaseRepository<User, BaseFilter>
    {
        Task<User> FindByEmailAsync(string email);
    }
}