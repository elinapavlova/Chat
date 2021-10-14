using ChatAPI.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;

namespace ChatAPI.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("/api/[controller]")]
    public class UsersController : BaseController
    {
        private readonly IUserService _userService;

        public UsersController
        (
            IUserService userService
        )
        {
            _userService = userService;
        }
        
        //[HttpPost]
        //public async Task<ActionResult<ResultContainer<UserDto>>> CreateUserAsync(UserCredentialsDto user)
        //{
        //    return await ReturnResult<ResultContainer<UserDto>, UserDto>
        //        (_userService.CreateUserAsync(user));
        //}
    }
}