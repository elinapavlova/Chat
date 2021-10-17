using System.Threading.Tasks;
using ChatAPI.Controllers.Base;
using Infrastructure.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Dtos;
using Services.Contracts;

namespace ChatAPI.Controllers
{
    [ApiVersion("1.0")]
    [Route("/api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Cookies")]
    public class MessagesController : BaseController
    {
        private readonly IMessageService _messageService;

        public MessagesController(IMessageService messageService)
        {
            _messageService = messageService;
        }
        
        /// <summary>
        /// Create message
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<MessageResponseDto>> CreateMessageAsync(MessageResponseDto message)
            => await ReturnResult<ResultContainer<MessageResponseDto>, MessageResponseDto>
                (_messageService.CreateMessageAsync(message));

        /// <summary>
        /// Get message by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:int}")]
        public async Task<ActionResult<MessageResponseDto>> FindByIdAsync(int id)
            => await ReturnResult<ResultContainer<MessageResponseDto>, MessageResponseDto>
                (_messageService.FindByIdAsync(id));
    }
}