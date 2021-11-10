using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.Contracts;
using Infrastructure.Filter;
using Infrastructure.Result;
using Models;
using Models.Dtos.Room;
using Models.Error;
using Services.Contracts;

namespace Services
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public RoomService
        (
            IRoomRepository roomRepository, 
            IMapper mapper, 
            IUserService userService
        )
        {
            _roomRepository = roomRepository;
            _mapper = mapper;
            _userService = userService;
        }

        public async Task<ResultContainer<RoomDto>> Create(RoomDto roomDto)
        {
            var user = await _userService.GetById(roomDto.UserId);
            var room = await _roomRepository.GetById(roomDto.Id);
            var result = new ResultContainer<RoomDto>();
            
            // Если пользователь не существует
            if (user.ErrorType.HasValue)
            {
                result.ErrorType = ErrorType.NotFound;
                return result;
            }
            
            // Если комната уже существует
            if (room != null)
            {
                result.ErrorType = ErrorType.BadRequest;
                return result;
            }

            var newRoom = _mapper.Map<RoomDto, Room>(roomDto);
            newRoom.DateCreated = DateTime.Now;
            
            result = _mapper.Map<ResultContainer<RoomDto>>(await _roomRepository.Create(newRoom));
            return result;
        }

        public async Task<ResultContainer<ICollection<RoomDto>>> GetByName(string title, FilterPagingDto filter)
        {
            var result = new ResultContainer<ICollection<RoomDto>>();
            var rooms = await _roomRepository.GetByName(title, filter.PageNumber, filter.PageSize);

            if (rooms.Count == 0)
            {
                result.ErrorType = ErrorType.NotFound;
                return result;
            }

            result = _mapper.Map<ResultContainer<ICollection<RoomDto>>>(rooms);
            return result;
        }

        public async Task<ResultContainer<ICollection<RoomDto>>> GetFiltered(FilterPagingDto paging, FilterSortDto sort)
        {
            var result = new ResultContainer<ICollection<RoomDto>>();
            var rooms = await _roomRepository.GetFiltered(paging.PageNumber, paging.PageSize, 
                                                                             sort.ColumnName, sort.IsDescending);
            // Если на данной странице нет комнат
            if (rooms.Count == 0 && paging.PageNumber > 1)
            {
                result.ErrorType = ErrorType.NotFound;
                return result;
            }

            result = _mapper.Map<ResultContainer<ICollection<RoomDto>>>(rooms);
            return result;
        }

        public async Task<ResultContainer<RoomDto>> GetById(int id)
        {
            var result = new ResultContainer<RoomDto>();
            var room = await _roomRepository.GetById(id);

            if (room != null)
            {
                result = _mapper.Map<ResultContainer<RoomDto>>(room);
                return result;
            }
            
            result.ErrorType = ErrorType.NotFound;
            return result;
        }

        public async Task<ResultContainer<RoomResponseDto>> GetByIdWithChats(int id, FilterPagingDto filter)
        {
            var result = new ResultContainer<RoomResponseDto>();
            var room = await _roomRepository.GetByIdWithChats(id, filter.PageNumber, filter.PageSize);
            
            // Если комната не найдена или на данной странице нет чатов
            if (room == null || room.Chats == null && filter.PageNumber > 1)
            {
                result.ErrorType = ErrorType.NotFound;
                return result;
            }

            result = _mapper.Map<ResultContainer<RoomResponseDto>>(room);
            return result;
        }
    }
}