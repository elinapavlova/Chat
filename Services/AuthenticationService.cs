using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.Contracts;
using Infrastructure.Result;
using Models.Dtos.Token;
using Models.Dtos.User;
using Models.Error;
using Services.Contracts;

namespace Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher _passwordHasher;
        
        public AuthenticationService 
        (
            IUserService userService,  
            ITokenService tokenService,
            IMapper mapper,
            IPasswordHasher passwordHasher
        )
        {
            _tokenService = tokenService;
            _userService = userService;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
        }

        private async Task<ResultContainer<AccessTokenDto>> CreateAccessTokenAsync(UserCredentialsDto data)
        {
            var result = _mapper.Map<ResultContainer<AccessTokenDto>>(_tokenService.CreateAccessToken(data));
            return result;
        }

        public async Task<ResultContainer<AccessTokenDto>> RefreshTokenAsync(string refreshToken, string userEmail)
        {
            var result = new ResultContainer<AccessTokenDto>();
            var token = _tokenService.TakeRefreshToken(refreshToken);
            var user = await _userService.FindByEmailAsync(userEmail);
            
            // Если пользователь не существует
            if (user.Data == null)
            {
                result.ErrorType = ErrorType.NotFound;
                return result;
            }
            
            // Если refresh token недействительный или  время жизни токена истекло
            if (token == null || token.IsExpired())
            {
                result.ErrorType = ErrorType.BadRequest;
                return result;
            }
            
            result = _mapper.Map<ResultContainer<AccessTokenDto>>(_tokenService.CreateAccessToken(user.Data));
            return result;
        }

        public async Task<ResultContainer<AccessTokenDto>> Login(UserCredentialsDto data)
        {
            var user = await _userService.FindByEmailAsync(data.Email);
            var result = new ResultContainer<AccessTokenDto>();

            // Если пользователь не существует
            if (user.Data == null)
            {
                result.ErrorType = ErrorType.NotFound;
                return result;
            }

            var isEqualPasswords = _passwordHasher.PasswordMatches(data.Password, user.Data.Password);
            
            // Если введен неверный пароль
            if (!isEqualPasswords)
            {
                result.ErrorType = ErrorType.BadRequest;
                return result;
            }

            result = await CreateAccessTokenAsync(user.Data);
            return result;
        }
    }
}