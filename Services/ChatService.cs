using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.Contracts;
using Infrastructure.Filter;
using Infrastructure.Result;
using Models;
using Models.Dtos.Chat;
using Models.Error;
using Services.Contracts;

namespace Services
{
    public class ChatService : IChatService
    {
        private readonly IChatRepository _chatRepository;
        private readonly IMapper _mapper;
        private readonly IRoomService _roomService;
        private readonly IUserService _userService;

        public ChatService
        (
            IChatRepository chatRepository, 
            IMapper mapper, 
            IRoomService roomService,
            IUserService userService
        )
        {
            _chatRepository = chatRepository;
            _mapper = mapper;
            _roomService = roomService;
            _userService = userService;
        }

        public async Task<ResultContainer<ChatDto>> CreateChatAsync(ChatDto chatDto)
        {
            var room = await _roomService.FindByIdAsync(chatDto.RoomId);
            var user = await _userService.FindByIdAsync(chatDto.UserId);
            var chat = await _chatRepository.GetById(chatDto.Id);
            var result = new ResultContainer<ChatDto>();
            
            // Если комната/пользователь не существует или чат уже существует
            if (room.ErrorType.HasValue || user.ErrorType.HasValue || chat != null )
            {
                result.ErrorType = ErrorType.BadRequest;
                return result;
            }

            var newChat = _mapper.Map<ChatDto, Chat>(chatDto);
            newChat.DateCreated = DateTime.Now;
            
            result = _mapper.Map<ResultContainer<ChatDto>>(await _chatRepository.Create(newChat));
            return result;
        }

        public async Task<ResultContainer<ICollection<ChatDto>>> FindByNameAsync(string title, int page, int pageSize)
        {
            var filter = new BaseFilterDto
            {
                Paging = new FilterPagingDto {PageNumber = page, PageSize = pageSize}
            };
            
            var result = _mapper.Map<ResultContainer<ICollection<ChatDto>>>
                (await _chatRepository.FindByNameAsync(title, filter));
            
            if (result.Data.Count != 0)
                return result;

            result.ErrorType = ErrorType.NotFound;
            return result;
        }

        public async Task<ResultContainer<ChatDto>> FindByIdAsync(int id)
        {
            var result = new ResultContainer<ChatDto>();
            var chat = await _chatRepository.GetById(id);

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
            
            var filter = new BaseFilterDto
            {
                Paging = new FilterPagingDto {PageNumber = page, PageSize = pageSize}
            };
            
            var chat = await _chatRepository.GetByIdWithMessagesAsync(id, filter);

            // Если чат не найден или на данной странице нет сообщений
            if (chat == null || chat.Messages == null && page > 1)
            {
                result.ErrorType = ErrorType.NotFound;
                return result;
            }

            result = _mapper.Map<ResultContainer<ChatResponseDto>>(chat);
            return result;
        }

        public async Task<ResultContainer<int?>> CountChatsByRoomIdAsync(int roomId)
        {
            var result = _mapper.Map<ResultContainer<int?>>(await _chatRepository.Count(roomId));
            
            if (result.Data != null)
                return result;

            // Если комната не найдена
            result.ErrorType = ErrorType.NotFound;
            return result;
        }
    }
}