using System.Collections.Generic;
using System.Threading.Tasks;
using ChatAPI.Controllers.Base;
using Infrastructure.Result;
using Microsoft.AspNetCore.Authorization;
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
    [Route("/api/v{version:apiVersion}/[controller]/[action]")]
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
        /// <returns></returns>
        [HttpGet("{userId:int}")]
        public async Task<ActionResult<ResultContainer<ICollection<RoomDto>>>> GetRoomsUserIn(int userId)
            => await ReturnResult<ResultContainer<ICollection<RoomDto>>, ICollection<RoomDto>>
                (_userRoomService.GetRoomsUserIn(userId));
        
        /// <summary>
        /// Come in room
        /// </summary>
        /// <param name="userRoomDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<UserRoomDto>> ComeIn(UserRoomDto userRoomDto)
            => await ReturnResult<ResultContainer<UserRoomDto>, UserRoomDto>
                (_userRoomService.CreateUserRoomAsync(userRoomDto));

        /// <summary>
        /// Come out of room
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roomId"></param>
        /// <returns></returns>
        [HttpPost("{userId:int}/{roomId:int}")]
        public async Task<ActionResult<UserRoomResponseDto>> ComeOut(int userId, int roomId)
            => await ReturnResult<ResultContainer<UserRoomResponseDto>, UserRoomResponseDto>
                (_userRoomService.ComeOutOfRoom(userId, roomId));
        
        /// <summary>
        /// Get users list in room
        /// </summary>
        /// <param name="roomId"></param>
        /// <returns></returns>
        [HttpGet("{roomId:int}")]
        public async Task<ActionResult<ICollection<UserDto>>> GetUsersInRoom(int roomId)
            => await ReturnResult<ResultContainer<ICollection<UserDto>>, ICollection<UserDto>>
                (_userRoomService.GetUsersInRoom(roomId));
    }
}