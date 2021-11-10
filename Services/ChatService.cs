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

        public async Task<ResultContainer<ChatDto>> Create(ChatDto chatDto)
        {
            var room = await _roomService.GetById(chatDto.RoomId);
            var user = await _userService.GetById(chatDto.UserId);
            var chat = await _chatRepository.GetById(chatDto.Id);
            var result = new ResultContainer<ChatDto>();
            
            // Если комната или пользователь не существует
            if (room.ErrorType.HasValue || user.ErrorType.HasValue)
            {
                result.ErrorType = ErrorType.NotFound;
                return result;
            }
            
            // Если чат уже существует
            if (chat != null)
            {
                result.ErrorType = ErrorType.BadRequest;
                return result;
            }

            var newChat = _mapper.Map<ChatDto, Chat>(chatDto);
            newChat.DateCreated = DateTime.Now;
            
            result = _mapper.Map<ResultContainer<ChatDto>>(await _chatRepository.Create(newChat));
            return result;
        }

        public async Task<ResultContainer<ICollection<ChatDto>>> GetByName(string title, FilterPagingDto filter)
        {
            var result = new ResultContainer<ICollection<ChatDto>>();
            var chats = await _chatRepository.GetByName(title, filter.PageNumber, filter.PageSize);
            
            if (chats.Count == 0)
            {
                result.ErrorType = ErrorType.NotFound;
                return result;
            }

            result = _mapper.Map<ResultContainer<ICollection<ChatDto>>>(chats);
            return result;
        }

        public async Task<ResultContainer<ChatDto>> GetById(int id)
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

        public async Task<ResultContainer<ChatResponseDto>> GetByIdWithMessages(int id, FilterPagingDto filter)
        {
            var result = new ResultContainer<ChatResponseDto>();
            var chat = await _chatRepository.GetByIdWithMessages(id, filter.PageNumber, filter.PageSize);

            // Если чат не найден или на данной странице нет сообщений
            if (chat == null || chat.Messages == null && filter.PageNumber > 1)
            {
                result.ErrorType = ErrorType.NotFound;
                return result;
            }

            result = _mapper.Map<ResultContainer<ChatResponseDto>>(chat);
            return result;
        }

        public async Task<ResultContainer<int?>> CountChatsByRoomId(int roomId)
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