using System.Collections.Generic;
using System.Threading.Tasks;
using ChatAPI.Controllers.Base;
using Infrastructure.Filter;
using Infrastructure.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Dtos.Room;
using Services.Contracts;

namespace ChatAPI.Controllers
{
    [ApiVersion("1.0")]
    [Route("/api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize]
    public class RoomsController : BaseController
    {
        private readonly IRoomService _roomService;

        public RoomsController
        (
            IRoomService roomService
        )
        {
            _roomService = roomService;
        }
        
        /// <summary>
        /// Create a room
        /// </summary>
        /// <param name="room"></param>
        /// <response code="200">Return the room</response>
        /// <response code="400">If the room already exists</response>
        /// <response code="404">If the user doesn't exist</response>
        /// <response code="401">If the User wasn't authorized</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<RoomDto>> Create(RoomDto room)
            => await ReturnResult<ResultContainer<RoomDto>, RoomDto>(_roomService.Create(room));

        /// <summary>
        /// Get rooms list by name
        /// </summary>
        /// <param name="name"></param>
        /// <param name="filter"></param>
        /// <response code="200">Return the room</response>
        /// <response code="404">If the rooms don't exist</response>
        /// <response code="401">If the User wasn't authorized</response>
        [HttpGet("{name}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ICollection<RoomDto>>> GetByName(string name, [FromQuery] FilterPagingDto filter)
            => await ReturnResult<ResultContainer<ICollection<RoomDto>>, ICollection<RoomDto>>
                (_roomService.GetByName(name, filter));

        /// <summary>
        /// Get chats list in the room by page
        /// </summary>
        /// <param name="id"></param>
        /// <param name="filter"></param>
        /// <response code="200">Return the room with chats</response>
        /// <response code="404">If the room wasn't found</response>
        /// <response code="401">If the User wasn't authorized</response>
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<RoomResponseDto>> GetByIdWithChats(int id, [FromQuery] FilterPagingDto filter)
            => await ReturnResult<ResultContainer<RoomResponseDto>, RoomResponseDto>
                (_roomService.GetByIdWithChats(id, filter));

        /// <summary>
        /// Get filtered rooms list by page
        /// </summary>
        /// <response code="200">Return the rooms list</response>
        /// <response code="404">If the room doesn't exist or there are not chats at this page</response>
        /// <response code="401">If the User wasn't authorized</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ICollection<RoomDto>>> GetFiltered
            ([FromQuery] FilterPagingDto paging, [FromQuery] FilterSortDto sort)
            => await ReturnResult<ResultContainer<ICollection<RoomDto>>, ICollection<RoomDto>>
                (_roomService.GetFiltered(paging, sort));
    }
}