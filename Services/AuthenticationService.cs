using System.Collections.Generic;
using System.Security.Claims;
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

        private async Task<ResultContainer<AccessTokenDto>> CreateAccessTokenAsync(string email, string password)
        {
            var user = await _userService.FindByEmailAsync(email);
            var result = new ResultContainer<AccessTokenDto>();
            
            if (user.Data == null)
            {
                result.ErrorType = ErrorType.NotFound;
                return result;
            }

            var isEqualPasswords = _passwordHasher.PasswordMatches(password, user.Data.Password);
            
            if (!isEqualPasswords)
            {
                result.ErrorType = ErrorType.BadRequest;
                return result;
            }

            result = _mapper.Map<ResultContainer<AccessTokenDto>>(_tokenService.CreateAccessToken(user.Data));
            return result;
        }

        public async Task<ResultContainer<AccessTokenDto>> RefreshTokenAsync(string refreshToken, string userEmail)
        {
            var result = new ResultContainer<AccessTokenDto>();
            var token = _tokenService.TakeRefreshToken(refreshToken);
            var user = await _userService.FindByEmailAsync(userEmail);
            
            if (token == null || token.IsExpired() || user.Data == null)
            {
                result.ErrorType = ErrorType.BadRequest;
                return result;
            }
            
            result = _mapper.Map<ResultContainer<AccessTokenDto>>(_tokenService.CreateAccessToken(user.Data));
            return result;
        }

        public async Task<ClaimsPrincipal> CreatePrincipals(UserCredentialsDto user, string token)
        {
            var claims = new List<Claim>
            {
                new (ClaimsIdentity.DefaultNameClaimType, user.Email),
                new ("Token", token)
            };

            var id = new ClaimsIdentity
                (claims, "ApplicationCookie", 
                    ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            
            return new ClaimsPrincipal(id);
        }

        public async Task<ResultContainer<AccessTokenDto>> Login(UserCredentialsDto data)
        {
            var user = await _userService.FindByEmailAsync(data.Email);
            var result = new ResultContainer<AccessTokenDto>();

            if (user.ErrorType.HasValue)
            {
                result.ErrorType = ErrorType.BadRequest;
                return result;
            }
            
            result = await CreateAccessTokenAsync(data.Email, data.Password);
            return result;
        }
    }
}