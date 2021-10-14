using Models.Dtos.User;
using Models.Token;

namespace Services.Contracts
{
    public interface ITokenService
    {
        AccessToken CreateAccessToken(UserCredentialsDto user);
        RefreshToken TakeRefreshToken(string token);
    }
}