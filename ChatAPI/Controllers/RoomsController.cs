using System.Collections.Generic;
using System.Threading.Tasks;
using ChatAPI.Controllers.Base;
using Infrastructure.Options;
using Infrastructure.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Dtos.Room;
using Services.Contracts;

namespace ChatAPI.Controllers
{
    [ApiVersion("1.0")]
    [Route("/api/v{version:apiVersion}/[controller]/[action]")]
    [ApiController]
    //[Authorize]
    public class RoomsController : BaseController
    {
        private readonly IRoomService _roomService;
        private readonly int _pageSize;

        public RoomsController
        (
            IRoomService roomService, 
            PagingOptions pagingOptions
        )
        {
            _roomService = roomService;
            _pageSize = pagingOptions.DefaultPageSize;
        }
        
        /// <summary>
        /// Create a room
        /// </summary>
        /// <param name="room"></param>
        /// <response code="200">Return the room</response>
        /// <response code="400">If the room already exists</response>
        /// <response code="401">If the User wasn't authorized</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<RoomDto>> CreateRoomAsync(RoomDto room)
            => await ReturnResult<ResultContainer<RoomDto>, RoomDto>(_roomService.CreateRoomAsync(room));

        /// <summary>
        /// Get rooms list by name
        /// </summary>
        /// <param name="name"></param>
        /// <param name="page"></param>
        /// <response code="200">Return the room</response>
        /// <response code="404">If the rooms don't exist</response>
        /// <response code="401">If the User wasn't authorized</response>
        [HttpGet("{name}/{page:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ICollection<RoomDto>>> FindByNameAsync(string name, int page)
            => await ReturnResult<ResultContainer<ICollection<RoomDto>>, ICollection<RoomDto>>
                (_roomService.FindByNameAsync(name, page, _pageSize));

        /// <summary>
        /// Get chats list in the room by page
        /// </summary>
        /// <param name="id"></param>
        /// <param name="page"></param>
        /// <response code="200">Return the room with chats</response>
        /// <response code="404">If the room wasn't found</response>
        /// <response code="401">If the User wasn't authorized</response>
        [HttpGet("{id:int}/{page:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<RoomResponseDto>> GetByIdWithChatsAsync(int id, int page)
            => await ReturnResult<ResultContainer<RoomResponseDto>, RoomResponseDto>
                (_roomService.GetByIdWithChatsAsync(id, page, _pageSize));

        /// <summary>
        /// Get filtered rooms list by page
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="columnName"></param>
        /// <param name="isDescending"></param>
        /// <response code="200">Return the rooms list</response>
        /// <response code="404">If the room doesn't exist or there are not chats at this page</response>
        /// <response code="401">If the User wasn't authorized</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ICollection<RoomDto>>> GetPageAsync
            (int page, int pageSize, string columnName, bool isDescending)
            => await ReturnResult<ResultContainer<ICollection<RoomDto>>, ICollection<RoomDto>>
                (_roomService.GetPageAsync(page, pageSize, columnName, isDescending));
    }
}