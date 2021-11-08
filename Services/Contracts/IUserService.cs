using System.Threading.Tasks;
using Infrastructure.Result;
using Models.Dtos.User;

namespace Services.Contracts
{
    public interface IUserService
    {
        Task<ResultContainer<UserDto>> Create(UserCredentialsDto user);
        Task<ResultContainer<UserCredentialsDto>> GetByEmail(string email);
        Task<ResultContainer<UserCredentialsDto>> GetById(int id);
    }
}