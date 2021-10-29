using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.Contracts;
using Infrastructure.Options;
using Infrastructure.Result;
using Models;
using Models.Dtos.Chat;
using Models.Error;
using Services.Contracts;

namespace Services
{
    public class ChatService : IChatService
    {
        private readonly IChatRepository _repository;
        private readonly IMapper _mapper;
        private readonly IRoomService _roomService;
        private readonly IUserService _userService;
        private readonly PagingOptions _pagingOptions;

        public ChatService
        (
            IChatRepository repository, 
            IMapper mapper, 
            IRoomService roomService,
            IUserService userService,
            PagingOptions pagingOptions
        )
        {
            _repository = repository;
            _mapper = mapper;
            _roomService = roomService;
            _userService = userService;
            _pagingOptions = pagingOptions;
        }

        public async Task<ResultContainer<ChatDto>> CreateChatAsync(ChatDto chatDto)
        {
            var room = await _roomService.FindByIdAsync(chatDto.RoomId);
            var user = await _userService.FindByIdAsync(chatDto.UserId);
            var chat = await _repository.GetById(chatDto.Id);
            var result = new ResultContainer<ChatDto>();
            
            // Если комната/пользователь не существует или чат уже существует
            if (room.ErrorType.HasValue || chat != null || user.ErrorType.HasValue)
            {
                result.ErrorType = ErrorType.BadRequest;
                return result;
            }

            var newChat = _mapper.Map<ChatDto, Chat>(chatDto);
            newChat.DateCreated = DateTime.Now;
            
            result = _mapper.Map<ResultContainer<ChatDto>>(await _repository.Create(newChat));
            return result;
        }

        public async Task<ResultContainer<ICollection<ChatDto>>> FindByNameAsync(string title, int page, int pageSize)
        {
            var result = new ResultContainer<ICollection<ChatDto>>();
            
            if (page < 1)
                page = _pagingOptions.DefaultPageNumber;

            var chats = await _repository.FindByNameAsync(title, page, pageSize);
            if (chats.Count == 0)
            {
                result.ErrorType = ErrorType.NotFound;
                return result;
            }
            
            result = _mapper.Map<ResultContainer<ICollection<ChatDto>>>(chats);
            return result;
        }

        public async Task<ResultContainer<ChatDto>> FindByIdAsync(int id)
        {
            var result = new ResultContainer<ChatDto>();
            var chat = await _repository.GetById(id);

            if (chat == null)
            {
                result.ErrorType = ErrorType.NotFound;
                return result;
            }

            result = _mapper.Map<ResultContainer<ChatDto>>(chat);
            return result;
        }

        public async Task<ResultContainer<ChatResponseDto>> GetByIdWithMessagesAsync(int id, int page, int pageSize)
        {
            var result = new ResultContainer<ChatResponseDto>();
            var chat = await _repository.GetByIdWithMessagesAsync(id, page, pageSize);

            if (page < 1)
                page = _pagingOptions.DefaultPageNumber;

            // Если чат не найден
            if (chat == null)
            {
                result.ErrorType = ErrorType.BadRequest;
                return result;
            }

            // Если на данной странице нет сообщений
            if (chat.Messages == null && page > 1)
            {
                result.ErrorType = ErrorType.NotFound;
                return result;
            }

            result = _mapper.Map<ResultContainer<ChatResponseDto>>(chat);
            return result;
        }
    }
}