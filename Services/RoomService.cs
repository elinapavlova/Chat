﻿using System;
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

        public async Task<ResultContainer<ICollection<RoomDto>>> GetByName(string title, int page, int pageSize)
        {
            var filter = new BaseFilterDto
            {
                Paging = new FilterPagingDto { PageNumber = page, PageSize = pageSize }
            };
            
            var result = _mapper.Map<ResultContainer<ICollection<RoomDto>>>
                (await _roomRepository.GetByName(title, filter));
            
            if (result.Data.Count != 0)
                return result;

            result.ErrorType = ErrorType.NotFound;
            return result;
        }

        public async Task<ResultContainer<ICollection<RoomDto>>> GetPage
            (int page, int pageSize, string columnName, bool isDescending)
        {
            var filter = new BaseFilterDto
            {
                Paging = new FilterPagingDto {PageNumber = page, PageSize = pageSize},
                Sort = new FilterSortDto {ColumnName = columnName, IsDescending = isDescending}
            };
            
            var result = _mapper.Map<ResultContainer<ICollection<RoomDto>>>(await _roomRepository.GetFiltered(filter));
            
            if (result.Data.Count != 0) 
                return result;
            
            result.ErrorType = ErrorType.NotFound;
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

        public async Task<ResultContainer<RoomResponseDto>> GetByIdWithChats(int id, int page, int pageSize)
        {
            var result = new ResultContainer<RoomResponseDto>();

            var filter = new BaseFilterDto
            {
                Paging = new FilterPagingDto { PageNumber = page, PageSize = pageSize }
            };

            var room = await _roomRepository.GetByIdWithChats(id, filter);
            
            // Если комната не найдена или на данной странице нет чатов
            if (room == null || room.Chats == null && page > 1)
            {
                result.ErrorType = ErrorType.NotFound;
                return result;
            }

            result = _mapper.Map<ResultContainer<RoomResponseDto>>(room);
            return result;
        }
    }
}