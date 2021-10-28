using System.Collections.Generic;
using System.Threading.Tasks;
using ChatAPI.Controllers.Base;
using Infrastructure.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Dtos.Chat;
using Models.Dtos.User;
using Models.Dtos.UserChat;
using Services.Contracts;

namespace ChatAPI.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Authorize]
    [Route("/api/v{version:apiVersion}/[controller]/[action]")]
    public class UsersChatsController : BaseController
    {
        private readonly IUserChatService _userChatService;

        public UsersChatsController
        (
            IUserChatService userChatService
        )
        {
            _userChatService = userChatService;
        }
        
        /// <summary>
        /// Get chat which user in
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("{userId:int}")]
        public async Task<ActionResult<ResultContainer<ICollection<ChatDto>>>> GetChatUserIn(int userId)
            => await ReturnResult<ResultContainer<ICollection<ChatDto>>, ICollection<ChatDto>>
                (_userChatService.GetChatsUserIn(userId));
        
        /// <summary>
        /// Come in chat
        /// </summary>
        /// <param name="userChatDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<UserChatDto>> ComeIn(UserChatDto userChatDto)
            => await ReturnResult<ResultContainer<UserChatDto>, UserChatDto>
                (_userChatService.CreateUserChatAsync(userChatDto));

        /// <summary>
        /// Come out of chat
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="chatId"></param>
        /// <returns></returns>
        [HttpPost("{userId:int}/{chatId:int}")]
        public async Task<ActionResult<UserChatResponseDto>> ComeOut(int userId, int chatId)
            => await ReturnResult<ResultContainer<UserChatResponseDto>, UserChatResponseDto>
                (_userChatService.ComeOutOfChat(userId, chatId));
        
        /// <summary>
        /// Get users list in chat
        /// </summary>
        /// <param name="chatId"></param>
        /// <returns></returns>
        [HttpGet("{chatId:int}")]
        public async Task<ActionResult<ICollection<UserDto>>> GetUsersInChat(int chatId)
            => await ReturnResult<ResultContainer<ICollection<UserDto>>, ICollection<UserDto>>
                (_userChatService.GetUsersInChat(chatId));
    }
}