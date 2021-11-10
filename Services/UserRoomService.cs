using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.Contracts;
using Infrastructure.Filter;
using Infrastructure.Result;
using Models;
using Models.Dtos.Room;
using Models.Dtos.User;
using Models.Dtos.UserRoom;
using Models.Error;
using Services.Contracts;

namespace Services
{
    public class UserRoomService : IUserRoomService
    {
        private readonly IUserRoomRepository _userRoomRepository;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IRoomService _roomService;

        public UserRoomService
        (
            IUserRoomRepository userRoomRepository,
            IMapper mapper,
            IUserService userService,
            IRoomService roomService
        )
        {
            _userRoomRepository = userRoomRepository;
            _mapper = mapper;
            _userService = userService;
            _roomService = roomService;
        }
        
        /// <summary>
        /// Добавить пользователя в комнату
        /// </summary>
        /// <param name="userRoomDto"></param>
        /// <returns></returns>
        public async Task<ResultContainer<UserRoomDto>> Create(UserRoomDto userRoomDto)
        {
            var result = new ResultContainer<UserRoomDto>();
            var userInRoom = await CheckUserInRoom(userRoomDto.UserId, userRoomDto.RoomId);
            
            // Если пользователя/комнаты не существует
            if (userInRoom.ErrorType.HasValue)
            {
                result.ErrorType = ErrorType.NotFound;
                return result;
            }

            // Если пользователь уже состоит в комнате
            if (userInRoom.Data != null)
            {
                result.ErrorType = ErrorType.BadRequest;
                return result;
            }

            var newUserRoom = _mapper.Map<UserRoomDto, UserRoom>(userRoomDto);
            newUserRoom.DateCreated = DateTime.Now;
            newUserRoom.DateComeOut = null;

            result = _mapper.Map<ResultContainer<UserRoomDto>>(await _userRoomRepository.Create(newUserRoom));
            return result;

        }

        /// <summary>
        /// Получить список комнат, в которых состоит пользователь
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<ResultContainer<ICollection<RoomDto>>> GetRoomsUserIn(int userId, FilterPagingDto filter)
        {
            var result = new ResultContainer<ICollection<RoomDto>>();
            var user = await _userService.GetById(userId);
            
            // Если пользователь не существует
            if (user.ErrorType.HasValue)
            {
                result.ErrorType = ErrorType.NotFound;
                return result;
            }

            var rooms = await _userRoomRepository.GetRoomsByUserId(userId, filter.PageNumber, filter.PageSize);
            
            // Если на странице нет комнат
            if (rooms.Count == 0 && filter.PageNumber > 1)
            {
                result.ErrorType = ErrorType.NotFound;
                return result;
            }

            result = _mapper.Map<ResultContainer<ICollection<RoomDto>>>(rooms);
            return result;
        }

        /// <summary>
        /// Получить список пользователей в комнате
        /// </summary>
        /// <param name="roomId"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<ResultContainer<ICollection<UserDto>>> GetUsersByRoomId(int roomId, FilterPagingDto filter)
        {
            var result = new ResultContainer<ICollection<UserDto>>();
            var room = await _roomService.GetById(roomId);
            
            // Если комната не существует
            if (room.ErrorType.HasValue)
            {
                result.ErrorType = ErrorType.NotFound;
                return result;
            }

            var users = await _userRoomRepository.GetUsersByRoomId(roomId, filter.PageNumber, filter.PageSize);

            result = _mapper.Map<ResultContainer<ICollection<UserDto>>>(users);
            return result;
        }

        /// <summary>
        /// Проверить, состоит ли пользователь в комнате
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roomId"></param>
        /// <returns></returns>
        public async Task<ResultContainer<UserRoomDto>> CheckUserInRoom(int userId, int roomId)
        {
            var result = new ResultContainer<UserRoomDto>();
            var user = await _userService.GetById(userId);
            var room = await _roomService.GetById(roomId);

            // Если пользователь или комната не существует
            if (user.ErrorType.HasValue || room.ErrorType.HasValue)
            {
                result.ErrorType = ErrorType.NotFound;
                return result;
            }

            var userRoom = await _userRoomRepository.CheckUserInRoom(userId, roomId);
            
            // Если пользователь не состоит в комнате
            if (userRoom == null)
                return result;

            result = _mapper.Map<ResultContainer<UserRoomDto>>(userRoom);
            return result;
        }

        /// <summary>
        /// Выйти из комнаты
        /// </summary>
        /// <returns></returns>
        public async Task<ResultContainer<UserRoomResponseDto>> ComeOutOfRoom(int userId, int roomId)
        {
            var result = new ResultContainer<UserRoomResponseDto>();
            var userRoom = await _userRoomRepository.ComeOutOfRoom(userId, roomId);

            // Если пользователь состоит в комнате
            if (userRoom != null)
            {
                result = _mapper.Map<ResultContainer<UserRoomResponseDto>>(userRoom);
                return result;
            }

            result.ErrorType = ErrorType.NotFound;
            return result;
        }
    }
}