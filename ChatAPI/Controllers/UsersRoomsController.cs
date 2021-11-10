using System.Collections.Generic;
using System.Threading.Tasks;
using ChatAPI.Controllers.Base;
using Infrastructure.Filter;
using Infrastructure.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Dtos.Room;
using Models.Dtos.User;
using Models.Dtos.UserRoom;
using Services.Contracts;

namespace ChatAPI.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Authorize]
    [Route("/api/v{version:apiVersion}/[controller]")]
    public class UsersRoomsController : BaseController
    {
        private readonly IUserRoomService _userRoomService;

        public UsersRoomsController
        (
            IUserRoomService userRoomService
        )
        {
            _userRoomService = userRoomService;
        }

        /// <summary>
        /// Get rooms which user in
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="filter"></param>
        /// <response code="200">Return rooms which user in</response>
        /// <response code="404">If there are not rooms at this page or the user doesn't exist</response>
        /// <response code="401">If the User wasn't authorized</response>
        [HttpGet("{userId:int}/rooms")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ResultContainer<ICollection<RoomDto>>>> GetRoomsUserIn
            (int userId, [FromQuery] FilterPagingDto filter)
            => await ReturnResult<ResultContainer<ICollection<RoomDto>>, ICollection<RoomDto>>
                (_userRoomService.GetRoomsUserIn(userId, filter));
        
        /// <summary>
        /// Come in room
        /// </summary>
        /// <param name="userRoomDto"></param>
        /// <response code="200">Return user id and room id</response>
        /// <response code="404">If the room or user doesn't exist</response>
        /// <response code="400">If the user is already in room</response>
        /// <response code="401">If the User wasn't authorized</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<UserRoomDto>> ComeIn(UserRoomDto userRoomDto)
            => await ReturnResult<ResultContainer<UserRoomDto>, UserRoomDto>
                (_userRoomService.Create(userRoomDto));

        /// <summary>
        /// Come out of room
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roomId"></param>
        /// <response code="200">Return user id, room id and exit date</response>
        /// <response code="404">If the user is not in room</response>
        /// <response code="401">If the User wasn't authorized</response>
        [HttpPost("{userId:int}/{roomId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<UserRoomResponseDto>> ComeOut(int userId, int roomId)
            => await ReturnResult<ResultContainer<UserRoomResponseDto>, UserRoomResponseDto>
                (_userRoomService.ComeOutOfRoom(userId, roomId));

        /// <summary>
        /// Get users list in room
        /// </summary>
        /// <param name="roomId"></param>
        /// <param name="filter"></param>
        /// <response code="200">Return users list</response>
        /// <response code="404">If the room doesn't exist</response>
        /// <response code="401">If the User wasn't authorized</response>
        [HttpGet("{roomId:int}/users")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ICollection<UserDto>>> GetUsersByRoomId(int roomId, [FromQuery] FilterPagingDto filter)
            => await ReturnResult<ResultContainer<ICollection<UserDto>>, ICollection<UserDto>>
                (_userRoomService.GetUsersByRoomId(roomId, filter));
    }
}