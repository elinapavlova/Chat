using System.Threading.Tasks;
using ChatAPI.Controllers.Base;
using Infrastructure.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Dtos.Message;
using Services.Contracts;

namespace ChatAPI.Controllers
{
    [ApiVersion("1.0")]
    [Route("/api/v{version:apiVersion}/[controller]")]
    [Authorize]
    [ApiController]
    public class MessagesController : BaseController
    {
        private readonly IMessageService _messageService;

        public MessagesController(IMessageService messageService)
        {
            _messageService = messageService;
        }
        
        /// <summary>
        /// Create a message
        /// </summary>
        /// <param name="message"></param>
        /// <response code="200">Return the message</response>
        /// <response code="204">If message is empty</response>
        /// <response code="400">If files weren't uploaded successfully</response>
        /// <response code="404">If the user is not in chat</response>
        /// <response code="401">If the User wasn't authorized</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<MessageResponseDto>> Create([FromForm] MessageRequestDto message)
            => await ReturnResult<ResultContainer<MessageResponseDto>, MessageResponseDto>
                (_messageService.Create(message));

        /// <summary>
        /// Get message with files by Id
        /// </summary>
        /// <param name="id"></param>
        /// <response code="200">Return the message</response>
        /// <response code="404">If the message doesn't exist</response>
        /// <response code="401">If the User wasn't authorized</response>
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<MessageResponseDto>> GetById(int id)
            => await ReturnResult<ResultContainer<MessageResponseDto>, MessageResponseDto>
                (_messageService.GetById(id));
    }
}