using System.Collections.Generic;
using System.Threading.Tasks;
using ChatAPI.Controllers.Base;
using Infrastructure.Options;
using Infrastructure.Result;
using Microsoft.AspNetCore.Authorization;
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
        private readonly int _pageSize;

        public RoomsController(IRoomService roomService, PagingOptions pagingOptions)
        {
            _roomService = roomService;
            _pageSize = pagingOptions.DefaultPageSize;
        }
        
        [HttpPost]
        public async Task<ActionResult<RoomDto>> CreateRoomAsync(RoomDto room)
            => await ReturnResult<ResultContainer<RoomDto>, RoomDto>(_roomService.CreateRoomAsync(room));

        /// <summary>
        /// Get rooms list by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet("{name}")]
        public async Task<ActionResult<ICollection<RoomDto>>> FindByNameAsync(string name)
            => await ReturnResult<ResultContainer<ICollection<RoomDto>>, ICollection<RoomDto>>
                (_roomService.FindByNameAsync(name));

        /// <summary>
        /// Paging messages list in the room
        /// </summary>
        /// <param name="id"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet("{id:int}/{page:int}")]
        public async Task<ActionResult<RoomResponseDto>> GetByIdWithMessagesAsync(int id, int page)
            => await ReturnResult<ResultContainer<RoomResponseDto>, RoomResponseDto>
                (_roomService.GetByIdWithMessagesAsync(id, page, _pageSize));

        /// <summary>
        /// Paging rooms list
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet("{page:int}")]
        public async Task<ActionResult<ICollection<RoomDto>>> GetAllAsync(int page)
            => await ReturnResult<ResultContainer<ICollection<RoomDto>>, ICollection<RoomDto>>
                (_roomService.GetPageAsync(page, _pageSize));
    }
}