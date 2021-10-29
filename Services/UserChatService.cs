using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.Contracts;
using Infrastructure.Options;
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
        private readonly PagingOptions _pagingOptions;

        public UserChatService
        (
            IUserChatRepository userChatRepository,
            IMapper mapper,
            IUserService userService,
            IChatService chatService,
            IUserRoomService userRoomService,
            IRoomService roomService,
            PagingOptions pagingOptions
        )
        {
            _userChatRepository = userChatRepository;
            _mapper = mapper;
            _userService = userService;
            _chatService = chatService;
            _userRoomService = userRoomService;
            _roomService = roomService;
            _pagingOptions = pagingOptions;
        }
        
        /// <summary>
        /// Добавить пользователя в чат
        /// </summary>
        /// <param name="userChatDto"></param>
        /// <returns></returns>
        public async Task<ResultContainer<UserChatDto>> CreateUserChatAsync(UserChatDto userChatDto)
        {
            var chat = await _chatService.FindByIdAsync(userChatDto.ChatId);
            var result = new ResultContainer<UserChatDto>();
            
            // Если чат не существует
            if (chat.ErrorType.HasValue)
            {
                result.ErrorType = ErrorType.BadRequest;
                return result;
            }
            
            var room = await _roomService.FindByIdAsync(chat.Data.RoomId);
            var userInChat = await CheckUserInChat(userChatDto.UserId, userChatDto.ChatId);
            var userInRoom = 
                await _userRoomService.CheckUserInRoom(userChatDto.UserId, room.Data.Id);
            
            // Если пользователь уже состоит в чате или не состоит в комнате
            if (userInChat.Data != null || userInRoom.ErrorType.HasValue)
            {
                result.ErrorType = ErrorType.BadRequest;
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
        /// <returns></returns>
        public async Task<ResultContainer<ICollection<ChatDto>>> GetChatsUserIn(int userId, int page, int pageSize)
        {
            var result = new ResultContainer<ICollection<ChatDto>>();
            var user = await _userService.FindByIdAsync(userId);
            
            if (user.ErrorType.HasValue)
            {
                result.ErrorType = ErrorType.NotFound;
                return result;
            }
            
            if (page < 1)
                page = _pagingOptions.DefaultPageNumber;
            
            if (pageSize < 1)
                pageSize = _pagingOptions.DefaultPageSize;

            var chats = await _userChatRepository.GetChatsByUserId(userId, page, pageSize);

            result = _mapper.Map<ResultContainer<ICollection<ChatDto>>>(chats);
            return result;
        }

        /// <summary>
        /// Получить список пользователей в чате
        /// </summary>
        /// <param name="chatId"></param>
        /// <returns></returns>
        public async Task<ResultContainer<ICollection<UserDto>>> GetUsersInChat(int chatId)
        {
            var result = new ResultContainer<ICollection<UserDto>>();
            var chat = await _chatService.FindByIdAsync(chatId);
            
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
        /// <returns></returns>
        public async Task<ResultContainer<UserChatDto>> CheckUserInChat(int userId, int chatId)
        {
            var result = new ResultContainer<UserChatDto>();
            var userChat = await _userChatRepository.CheckUserInChat(userId, chatId);
            
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
        /// <returns></returns>
        public async Task<ResultContainer<UserChatResponseDto>> ComeOutOfChat(int userId, int chatId)
        {
            var result = new ResultContainer<UserChatResponseDto>();
            var userChat = await _userChatRepository.ComeOutOfChat(userId, chatId);
            
            if (userChat == null)
            {
                result.ErrorType = ErrorType.BadRequest;
                return result;
            }

            result = _mapper.Map<ResultContainer<UserChatResponseDto>>(userChat);
            return result;
        }
    }
}