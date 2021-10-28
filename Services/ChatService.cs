using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.Contracts;
using Infrastructure.Filter;
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

        public async Task<ResultContainer<ICollection<ChatDto>>> FindByNameAsync(string title)
        {
            var result = new ResultContainer<ICollection<ChatDto>>();
            var chats = await _repository.FindByNameAsync(title);

            if (chats == null)
            {
                result.ErrorType = ErrorType.NotFound;
                return result;
            }
            
            result = _mapper.Map<ResultContainer<ICollection<ChatDto>>>(chats);
            return result;
        }

        public async Task<ResultContainer<ICollection<ChatDto>>> GetPageAsync(int page, int pageSize, string columnName, bool isDescending)
        {
            if (page < 1)
                page = _pagingOptions.DefaultPageNumber;
            
            if (pageSize < 1)
                pageSize = _pagingOptions.DefaultPageSize;
            
            var filter = new BaseFilterDto
            {
                Paging = new FilterPagingDto {PageNumber = page, PageSize = pageSize},
                Sort = new FilterSortDto {ColumnName = columnName, IsDescending = isDescending}
            };
            
            var result = _mapper.Map<ResultContainer<ICollection<ChatDto>>>(await _repository.GetFiltered(filter));
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
            
            if (page < 1)
                page = _pagingOptions.DefaultPageNumber;

            var chat = await _repository.GetByIdWithMessagesAsync(id, page, pageSize);
            if (chat == null)
            {
                result.ErrorType = ErrorType.NotFound;
                return result;
            }

            result = _mapper.Map<ResultContainer<ChatResponseDto>>(chat);
            return result;
        }

    }
}