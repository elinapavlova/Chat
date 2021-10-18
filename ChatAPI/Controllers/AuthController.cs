using System.Threading.Tasks;
using ChatAPI.Controllers.Base;
using Infrastructure.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Dtos.Token;
using Models.Dtos.User;
using Services.Contracts;
using IAuthenticationService = Services.Contracts.IAuthenticationService;

namespace ChatAPI.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("/api/v{version:apiVersion}/[controller]/[action]")]
    public class AuthController : BaseController
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserService _userService;

        public AuthController(IAuthenticationService authenticationService, IUserService userService)
        {
            _authenticationService = authenticationService;
            _userService = userService;
        }

        /// <summary>
        /// Login
        /// </summary>
        /// <param name="userCredentials"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<ResultContainer<AccessTokenDto>>> LoginAsync(UserCredentialsDto userCredentials)
        {
            return await ReturnResult<ResultContainer<AccessTokenDto>, AccessTokenDto>
                (_authenticationService.Login(userCredentials));
        }
        
        /// <summary>
        /// Register
        /// </summary>
        /// <param name="userCredentials"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<ResultContainer<UserDto>>> RegisterAsync(UserCredentialsDto userCredentials)
        {
            return await ReturnResult<ResultContainer<UserDto>, UserDto>
                (_userService.CreateUserAsync(userCredentials));
        }

        /// <summary>
        /// Refresh token
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ResultContainer<AccessTokenDto>>> RefreshTokenAsync(RefreshTokenDto refreshToken)
        {
            return await ReturnResult<ResultContainer<AccessTokenDto>,AccessTokenDto>
            (_authenticationService.RefreshTokenAsync(refreshToken.RefreshToken, refreshToken.EmailUser));
        }
    }
}
