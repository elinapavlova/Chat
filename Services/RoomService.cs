using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.Contracts;
using Infrastructure.Filter;
using Infrastructure.Options;
using Infrastructure.Result;
using Models;
using Models.Dtos.Room;
using Models.Error;
using Services.Contracts;

namespace Services
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly PagingOptions _pagingOptions;

        public RoomService
        (
            IRoomRepository repository, 
            IMapper mapper, 
            IUserService userService,
            PagingOptions pagingOptions
        )
        {
            _repository = repository;
            _mapper = mapper;
            _userService = userService;
            _pagingOptions = pagingOptions;
        }

        public async Task<ResultContainer<RoomDto>> CreateRoomAsync(RoomDto roomDto)
        {
            var user = await _userService.FindByIdAsync(roomDto.UserId);
            var room = await _repository.GetById(roomDto.Id);
            var result = new ResultContainer<RoomDto>();
            
            if (user == null || room != null)
            {
                result.ErrorType = ErrorType.BadRequest;
                return result;
            }

            var newRoom = _mapper.Map<RoomDto, Room>(roomDto);
            newRoom.DateCreated = DateTime.Now;
            
            result = _mapper.Map<ResultContainer<RoomDto>>(await _repository.Create(newRoom));
            return result;
        }

        public async Task<ResultContainer<ICollection<RoomDto>>> FindByNameAsync(string title)
        {
            var result = new ResultContainer<ICollection<RoomDto>>();
            var room = await _repository.FindByNameAsync(title);

            if (room == null)
            {
                result.ErrorType = ErrorType.NotFound;
                return result;
            }
            
            result = _mapper.Map<ResultContainer<ICollection<RoomDto>>>(room);
            return result;
        }

        public async Task<ResultContainer<ICollection<RoomDto>>> GetPageAsync(int page, int pageSize, string columnName, bool isDescending)
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
            
            var result = _mapper.Map<ResultContainer<ICollection<RoomDto>>>(await _repository.GetFiltered(filter));
            return result;
        }

        public async Task<ResultContainer<RoomDto>> FindByIdAsync(int id)
        {
            var result = new ResultContainer<RoomDto>();
            var room = await _repository.GetById(id);

            if (room == null)
            {
                result.ErrorType = ErrorType.NotFound;
                return result;
            }

            result = _mapper.Map<ResultContainer<RoomDto>>(room);
            return result;
        }

        public async Task<ResultContainer<RoomResponseDto>> GetByIdWithMessagesAsync(int id, int page, int pageSize)
        {
            var result = new ResultContainer<RoomResponseDto>();
            
            if (page < 1)
                page = _pagingOptions.DefaultPageNumber;

            var room = await _repository.GetByIdWithMessagesAsync(id, page, pageSize);
            if (room == null)
            {
                result.ErrorType = ErrorType.NotFound;
                return result;
            }

            result = _mapper.Map<ResultContainer<RoomResponseDto>>(room);
            return result;
        }
    }
}