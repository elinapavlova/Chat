using System;
using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.Contracts;
using Infrastructure.Result;
using Models;
using Models.Dtos;
using Models.Error;
using Services.Contracts;

namespace Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IRoomService _roomService;
        
        public MessageService
        (
            IMessageRepository repository, 
            IMapper mapper, 
            IUserService userService,
            IRoomService roomService
        )
        {
            _repository = repository;
            _mapper = mapper;
            _userService = userService;
            _roomService = roomService;
        }
        
        public async Task<ResultContainer<MessageResponseDto>> CreateMessageAsync(MessageResponseDto messageDto)
        {
            var user = await _userService.FindByIdAsync(messageDto.IdUser);
            var room = await _roomService.FindByIdAsync(messageDto.IdRoom);
            var result = new ResultContainer<MessageResponseDto>();
            
            if (user.Data == null || room.Data == null || messageDto.Text == null)
            {
                result.ErrorType = ErrorType.BadRequest;
                return result;
            }
            
            var newMessage = _mapper.Map<MessageResponseDto, Message>(messageDto);
            newMessage.DateCreated = DateTime.Now;
            
            result = _mapper.Map<ResultContainer<MessageResponseDto>>(await _repository.Create(newMessage));
            return result;
        }

        public async Task<ResultContainer<MessageResponseDto>> FindByIdAsync(int id)
        {
            var result = new ResultContainer<MessageResponseDto>();
            var message = await _repository.GetById(id);
            
            if (id < 1 || message == null)
            {
                result.ErrorType = ErrorType.NotFound;
                return result;
            }
            
            result = _mapper.Map<ResultContainer<MessageResponseDto>>(message);
            return result;
        }
    }
}