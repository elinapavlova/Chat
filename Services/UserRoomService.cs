﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.Contracts;
using Infrastructure.Options;
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
        private readonly PagingOptions _pagingOptions;

        public UserRoomService
        (
            IUserRoomRepository userRoomRepository,
            IMapper mapper,
            IUserService userService,
            IRoomService roomService,
            PagingOptions pagingOptions
        )
        {
            _userRoomRepository = userRoomRepository;
            _mapper = mapper;
            _userService = userService;
            _roomService = roomService;
            _pagingOptions = pagingOptions;
        }
        
        /// <summary>
        /// Добавить пользователя в комнату
        /// </summary>
        /// <param name="userRoomDto"></param>
        /// <returns></returns>
        public async Task<ResultContainer<UserRoomDto>> CreateUserRoomAsync(UserRoomDto userRoomDto)
        {
            var result = new ResultContainer<UserRoomDto>();
            var userInRoom = await CheckUserInRoom(userRoomDto.UserId, userRoomDto.RoomId);
            
            // Если пользователя/комнаты не существует или пользователь уже состоит в комнате
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
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<ResultContainer<ICollection<RoomDto>>> GetRoomsUserIn(int userId, int page, int pageSize)
        {
            var result = new ResultContainer<ICollection<RoomDto>>();
            var user = await _userService.FindByIdAsync(userId);
            
            if (user.ErrorType.HasValue)
            {
                result.ErrorType = ErrorType.BadRequest;
                return result;
            }
            
            if (page < 1)
                page = _pagingOptions.DefaultPageNumber;
            
            if (pageSize < 1)
                pageSize = _pagingOptions.DefaultPageSize;
            
            var rooms = await _userRoomRepository.GetRoomsByUserId(userId, page, pageSize);
            if (rooms.Count == 0 && page > 1)
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
        /// <returns></returns>
        public async Task<ResultContainer<ICollection<UserDto>>> GetUsersInRoom(int roomId)
        {
            var result = new ResultContainer<ICollection<UserDto>>();
            var room = await _roomService.FindByIdAsync(roomId);
            
            if (room.ErrorType.HasValue)
            {
                result.ErrorType = ErrorType.NotFound;
                return result;
            }

            var users = await _userRoomRepository.GetUsersInRoom(roomId);

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
            var userRoom = await _userRoomRepository.CheckUserInRoom(userId, roomId);
            
            if (userRoom == null)
            {
                result.ErrorType = ErrorType.NotFound;
                return result;
            }

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
            
            if (userRoom == null)
            {
                result.ErrorType = ErrorType.BadRequest;
                return result;
            }

            result = _mapper.Map<ResultContainer<UserRoomResponseDto>>(userRoom);
            return result;
        }
    }
}