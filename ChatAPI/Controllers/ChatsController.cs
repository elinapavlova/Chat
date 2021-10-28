using System.Collections.Generic;
using System.Threading.Tasks;
using ChatAPI.Controllers.Base;
using Infrastructure.Options;
using Infrastructure.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Dtos.Chat;
using Services.Contracts;

namespace ChatAPI.Controllers
{
    [ApiVersion("1.0")]
    [Route("/api/v{version:apiVersion}/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class ChatsController : BaseController
    {
        private readonly IChatService _chatService;
        private readonly int _pageSize;

        public ChatsController(IChatService chatService, PagingOptions pagingOptions)
        {
            _chatService = chatService;
            _pageSize = pagingOptions.DefaultPageSize;
        }
        
        [HttpPost]
        public async Task<ActionResult<ChatDto>> CreateChatAsync(ChatDto room)
            => await ReturnResult<ResultContainer<ChatDto>, ChatDto>(_chatService.CreateChatAsync(room));

        /// <summary>
        /// Get chats list by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet("{name}")]
        public async Task<ActionResult<ICollection<ChatDto>>> FindByNameAsync(string name)
            => await ReturnResult<ResultContainer<ICollection<ChatDto>>, ICollection<ChatDto>>
                (_chatService.FindByNameAsync(name));

        /// <summary>
        /// Paging messages list in the chat
        /// </summary>
        /// <param name="id"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet("{id:int}/{page:int}")]
        public async Task<ActionResult<ChatResponseDto>> GetByIdWithMessagesAsync(int id, int page)
            => await ReturnResult<ResultContainer<ChatResponseDto>, ChatResponseDto>
                (_chatService.GetByIdWithMessagesAsync(id, page, _pageSize));

        /// <summary>
        /// Paging chats list
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="columnName"></param>
        /// <param name="isDescending"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<ICollection<ChatDto>>> GetAllAsync
            (int page, int pageSize, string columnName, bool isDescending)
            => await ReturnResult<ResultContainer<ICollection<ChatDto>>, ICollection<ChatDto>>
                (_chatService.GetPageAsync(page, pageSize, columnName, isDescending));
    }
}