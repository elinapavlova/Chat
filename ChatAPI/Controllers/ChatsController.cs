﻿using System.Collections.Generic;
using System.Threading.Tasks;
using ChatAPI.Controllers.Base;
using Infrastructure.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Dtos.Chat;
using Services.Contracts;

namespace ChatAPI.Controllers
{
    [ApiVersion("1.0")]
    [Route("/api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize]
    public class ChatsController : BaseController
    {
        private readonly IChatService _chatService;

        public ChatsController(IChatService chatService)
        {
            _chatService = chatService;
        }
        
        /// <summary>
        /// Create a chat
        /// </summary>
        /// <param name="chat"></param>
        /// <response code="200">Return the chat</response>
        /// <response code="400">If the chat already exists</response>
        /// <response code="404">If the user or room doesn't exist</response>
        /// <response code="401">If the User wasn't authorized</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ChatDto>> CreateChatAsync(ChatDto chat)
            => await ReturnResult<ResultContainer<ChatDto>, ChatDto>(_chatService.CreateChatAsync(chat));

        /// <summary>
        /// Get chats list by name
        /// </summary>
        /// <param name="name"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <response code="200">Return the chat</response>
        /// <response code="404">If the chats don't exist</response>
        /// <response code="401">If the User wasn't authorized</response>
        [HttpGet("{name}/{page:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ICollection<ChatDto>>> FindByNameAsync(string name, int page, int pageSize)
            => await ReturnResult<ResultContainer<ICollection<ChatDto>>, ICollection<ChatDto>>
                (_chatService.FindByNameAsync(name, page, pageSize));

        /// <summary>
        /// Get chat with messages by page
        /// </summary>
        /// <param name="id"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <response code="200">Return the chat</response>
        /// <response code="404">If the chat doesn't exist or there are not messages at this page</response>
        /// <response code="401">If the User wasn't authorized</response>
        [HttpGet("{id:int}/{page:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ChatResponseDto>> GetByIdWithMessagesAsync(int id, int page, int pageSize)
            => await ReturnResult<ResultContainer<ChatResponseDto>, ChatResponseDto>
                (_chatService.GetByIdWithMessagesAsync(id, page, pageSize));
    }
}