using System.Collections.Generic;
using System.Threading.Tasks;
using ChatAPI.Controllers.Base;
using Infrastructure.Filter;
using Infrastructure.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    [Route("/api/v{version:apiVersion}/[controller]")]
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
        /// Get chats which user in
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="dto"></param>
        /// <response code="200">Return chats which user in</response>
        /// <response code="404">If there are not chats at this page or the user doesn't exist</response>
        /// <response code="401">If the User wasn't authorized</response>
        [HttpGet("{userId:int}/chats")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ResultContainer<ICollection<ChatDto>>>> GetChatsUserIn
            (int userId, [FromQuery] FilterPagingDto dto)
            => await ReturnResult<ResultContainer<ICollection<ChatDto>>, ICollection<ChatDto>>
                (_userChatService.GetChatsUserIn(userId, dto));
        
        /// <summary>
        /// Come in chat
        /// </summary>
        /// <param name="userChatDto"></param>
        /// <response code="200">Return user id and chat id</response>
        /// <response code="404">If the user is not in room or room/user doesn't exist</response>
        /// <response code="400">If the user is already in chat</response>
        /// <response code="401">If the User wasn't authorized</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<UserChatDto>> ComeIn(UserChatDto userChatDto)
            => await ReturnResult<ResultContainer<UserChatDto>, UserChatDto>
                (_userChatService.Create(userChatDto));

        /// <summary>
        /// Come out of chat
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="chatId"></param>
        /// <response code="200">Return user id, chat id and exit date</response>
        /// <response code="404">If the user is not in chat</response>
        /// <response code="401">If the User wasn't authorized</response>
        [HttpPost("{userId:int}/{chatId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<UserChatResponseDto>> ComeOut(int userId, int chatId)
            => await ReturnResult<ResultContainer<UserChatResponseDto>, UserChatResponseDto>
                (_userChatService.ComeOutOfChat(userId, chatId));

        /// <summary>
        /// Get users list in chat
        /// </summary>
        /// <param name="chatId"></param>
        /// <param name="dto"></param>
        /// <response code="200">Return users list</response>
        /// <response code="404">If the chat doesn't exist</response>
        /// <response code="401">If the User wasn't authorized</response>
        [HttpGet("{chatId:int}/users")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ICollection<UserDto>>> GetUsersByChatId(int chatId, [FromQuery] FilterPagingDto dto)
            => await ReturnResult<ResultContainer<ICollection<UserDto>>, ICollection<UserDto>>
                (_userChatService.GetUsersByChatId(chatId, dto));
    }
}