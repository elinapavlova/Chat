using System.Threading.Tasks;
using ChatAPI.Controllers.Base;
using Infrastructure.Result;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
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
        [Route("/api/Login")]
        [HttpPost]
        public async Task<ActionResult<ResultContainer<AccessTokenDto>>> LoginAsync(UserCredentialsDto userCredentials)
        {
            return await ReturnResult<ResultContainer<AccessTokenDto>, AccessTokenDto>
                (AuthenticateAsync(userCredentials));
        }
        
        /// <summary>
        /// Register
        /// </summary>
        /// <param name="userCredentials"></param>
        /// <returns></returns>
        [Route("/api/Register")]
        [HttpPost]
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
        [Route("/api/Token/Refresh")]
        [HttpPost]
        [Authorize(AuthenticationSchemes = "Cookies")]
        public async Task<ActionResult<ResultContainer<AccessTokenDto>>> RefreshTokenAsync(RefreshTokenDto refreshToken)
        {
            return await ReturnResult<ResultContainer<AccessTokenDto>,AccessTokenDto>
            (_authenticationService.RefreshTokenAsync(refreshToken.RefreshToken, refreshToken.EmailUser));
        }

        /// <summary>
        /// Logout
        /// </summary>
        /// <returns></returns>
        [Route("/api/Logout")]
        [HttpPost]
        [Authorize(AuthenticationSchemes = "Cookies")]
        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok();
        }

        /// <summary>
        /// Authenticate user's login data 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private async Task<ResultContainer<AccessTokenDto>> AuthenticateAsync(UserCredentialsDto data)
        {
            var result = await _authenticationService.Login(data);

            if (result.ErrorType.HasValue)
                return result;
            
            var principal = await _authenticationService.CreatePrincipals(data, result.Data.AccessToken);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return result;
        }
    }
}
