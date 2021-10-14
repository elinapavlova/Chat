using System.Threading.Tasks;
using Infrastructure.Result;
using Models.Dtos.User;

namespace Services.Contracts
{
    public interface IUserService
    {
        Task<ResultContainer<UserDto>> CreateUserAsync(UserCredentialsDto user);
        Task<ResultContainer<UserCredentialsDto>> FindByEmailAsync(string email);
        Task<ResultContainer<UserCredentialsDto>> FindByIdAsync(int id);
    }
}