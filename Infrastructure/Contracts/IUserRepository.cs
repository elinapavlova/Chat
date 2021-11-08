using System.Threading.Tasks;
using Infrastructure.Contracts.Base;
using Models;

namespace Infrastructure.Contracts
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User> FindByEmailAsync(string email);
    }
}