using System.Threading.Tasks;
using ChatAPI.Controllers.Base;
using Infrastructure.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        /// Login user
        /// </summary>
        /// <param name="userCredentials"></param>
        /// <response code="200">Return access token, refresh tokens and expiration</response>
        /// <response code="404">If the user doesn't exist</response>
        /// <response code="400">If the password is not right</response>
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ResultContainer<AccessTokenDto>>> Login(UserCredentialsDto userCredentials)
        {
            return await ReturnResult<ResultContainer<AccessTokenDto>, AccessTokenDto>
                (_authenticationService.Login(userCredentials));
        }
        
        /// <summary>
        /// Register user
        /// </summary>
        /// <param name="userCredentials"></param>
        /// <response code="200">Return user Id and Email</response>
        /// <response code="400">If the user already exists or Email is not valid</response>
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ResultContainer<UserDto>>> Register(UserCredentialsDto userCredentials)
        {
            return await ReturnResult<ResultContainer<UserDto>, UserDto>
                (_userService.Create(userCredentials));
        }

        /// <summary>
        /// Get new token
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <response code="200">Return new access token, refresh token and expiration</response>
        /// <response code="400">If token lifetime is expired or refresh token isn't right</response>
        /// <response code="404">If user doesn't exist</response>
        /// <response code="401">If the User wasn't authorized</response>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ResultContainer<AccessTokenDto>>> RefreshToken(RefreshTokenDto refreshToken)
        {
            return await ReturnResult<ResultContainer<AccessTokenDto>,AccessTokenDto>
            (_authenticationService.RefreshToken(refreshToken.RefreshToken, refreshToken.EmailUser));
        }
    }
}
