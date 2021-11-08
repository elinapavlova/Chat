using System.Threading.Tasks;
using Infrastructure.Result;
using Models.Dtos.Token;
using Models.Dtos.User;

namespace Services.Contracts
{
    public interface IAuthenticationService
    {
        Task<ResultContainer<AccessTokenDto>> Login(UserCredentialsDto data);
        Task<ResultContainer<AccessTokenDto>> RefreshToken(string refreshToken, string userEmail);
    }
}