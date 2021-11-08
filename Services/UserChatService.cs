﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.Contracts;
using Infrastructure.Filter;
using Infrastructure.Result;
using Models;
using Models.Dtos.Chat;
using Models.Dtos.User;
using Models.Dtos.UserChat;
using Models.Error;
using Services.Contracts;

namespace Services
{
    public class UserChatService : IUserChatService
    {
        private readonly IUserChatRepository _userChatRepository;
        private readonly IUserRoomService _userRoomService;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IChatService _chatService;
        private readonly IRoomService _roomService;

        public UserChatService
        (
            IUserChatRepository userChatRepository,
            IMapper mapper,
            IUserService userService,
            IChatService chatService,
            IUserRoomService userRoomService,
            IRoomService roomService
        )
        {
            _userChatRepository = userChatRepository;
            _mapper = mapper;
            _userService = userService;
            _chatService = chatService;
            _userRoomService = userRoomService;
            _roomService = roomService;
        }
        
        /// <summary>
        /// Добавить пользователя в чат
        /// </summary>
        /// <param name="userChatDto"></param>
        public async Task<ResultContainer<UserChatDto>> CreateUserChatAsync(UserChatDto userChatDto)
        {
            var chat = await _chatService.FindByIdAsync(userChatDto.ChatId);
            var result = new ResultContainer<UserChatDto>();
            
            // Если чат не существует
            if (chat.ErrorType.HasValue)
            {
                result.ErrorType = ErrorType.NotFound;
                return result;
            }
            
            var room = await _roomService.FindByIdAsync(chat.Data.RoomId);
            var userInChat = await CheckUserInChat(userChatDto.UserId, userChatDto.ChatId);
            var userInRoom = 
                await _userRoomService.CheckUserInRoom(userChatDto.UserId, room.Data.Id);
            
            // Если пользователь уже состоит в чате или не состоит в комнате
            if (userInChat.Data != null || userInRoom.ErrorType.HasValue)
            {
                result.ErrorType = ErrorType.NotFound;
                return result;
            }

            var newUserChat = _mapper.Map<UserChatDto, UserChat>(userChatDto);
            newUserChat.DateCreated = DateTime.Now;
            newUserChat.DateComeOut = null;
            
            result = _mapper.Map<ResultContainer<UserChatDto>>(await _userChatRepository.Create(newUserChat));
            return result;
        }

        /// <summary>
        /// Получить список чатов, в которых состоит пользователь
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        public async Task<ResultContainer<ICollection<ChatDto>>> GetChatsUserIn(int userId, int page, int pageSize)
        {
            var result = new ResultContainer<ICollection<ChatDto>>();
            var user = await _userService.FindByIdAsync(userId);
            
            // Если пользователь не существует
            if (user.ErrorType.HasValue)
            {
                result.ErrorType = ErrorType.NotFound;
                return result;
            }
            
            var filter = new BaseFilterDto
            {
                Paging = new FilterPagingDto {PageNumber = page, PageSize = pageSize}
            };

            var chats = await _userChatRepository.GetChatsByUserId(userId, filter);
            
            // Если на странице нет чатов
            if (chats.Count == 0 && page > 1)
            {
                result.ErrorType = ErrorType.NotFound;
                return result;
            }

            result = _mapper.Map<ResultContainer<ICollection<ChatDto>>>(chats);
            return result;
        }

        /// <summary>
        /// Получить список пользователей в чате
        /// </summary>
        /// <param name="chatId"></param>
        public async Task<ResultContainer<ICollection<UserDto>>> GetUsersInChat(int chatId)
        {
            var result = new ResultContainer<ICollection<UserDto>>();
            var chat = await _chatService.FindByIdAsync(chatId);
            
            // Если чат не существует
            if (chat.ErrorType.HasValue)
            {
                result.ErrorType = ErrorType.NotFound;
                return result;
            }

            var users = await _userChatRepository.GetUsersInChat(chatId);

            result = _mapper.Map<ResultContainer<ICollection<UserDto>>>(users);
            return result;
        }

        /// <summary>
        /// Проверить, состоит ли пользователь в чате
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="chatId"></param>
        public async Task<ResultContainer<UserChatDto>> CheckUserInChat(int userId, int chatId)
        {
            var result = new ResultContainer<UserChatDto>();
            var userChat = await _userChatRepository.CheckUserInChat(userId, chatId);
            
            // Если пользователь не состоит в чате
            if (userChat == null)
            {
                result.ErrorType = ErrorType.NotFound;
                return result;
            }
            
            result = _mapper.Map<ResultContainer<UserChatDto>>(userChat);
            return result;
        }

        /// <summary>
        /// Выйти из чата
        /// </summary>
        public async Task<ResultContainer<UserChatResponseDto>> ComeOutOfChat(int userId, int chatId)
        {
            var result = _mapper.Map<ResultContainer<UserChatResponseDto>>
                (await _userChatRepository.ComeOutOfChat(userId, chatId));
            
            // Если пользователь состоит в чате
            if (result.Data != null)
                return result;

            result.ErrorType = ErrorType.NotFound;
            return result;
        }
    }
}