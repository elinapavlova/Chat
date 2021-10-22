using System;
using System.Threading.Tasks;
using System.Transactions;
using AutoMapper;
using Infrastructure.Contracts;
using Infrastructure.Result;
using Models;
using Models.Dtos.Message;
using Models.Error;
using Services.Contracts;

namespace Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IRoomService _roomService;
        private readonly IUploadService _uploadService;

        public MessageService
        (
            IMessageRepository repository, 
            IMapper mapper, 
            IUserService userService,
            IRoomService roomService,
            IUploadService uploadService
        )
        {
            _messageRepository = repository;
            _mapper = mapper;
            _userService = userService;
            _roomService = roomService;
            _uploadService = uploadService;
        }

        public async Task<ResultContainer<MessageResponseDto>> FindByIdAsync(int id)
        {
            var result = new ResultContainer<MessageResponseDto>();
            var message = await _messageRepository.GetById(id);
            
            if (id < 1 || message == null)
            {
                result.ErrorType = ErrorType.NotFound;
                return result;
            }
            
            result = _mapper.Map<ResultContainer<MessageResponseDto>>(message);
            return result;
        }
        
        public async Task<ResultContainer<MessageResponseDto>> CreateMessageAsync(MessageRequestDto message)
        {
            var resultMessage = await ValidateMessage(message);
            if (resultMessage.ErrorType.HasValue)
                return resultMessage;
            
            var mes = _mapper.Map<Message>(message);
            mes.DateCreated = DateTime.Now;
            
            switch (message.Files)
            {
                // Если нет файлов и есть текст сообщения - создать сообщение
                case null when !string.IsNullOrEmpty(message.Text):
                    resultMessage = _mapper.Map<ResultContainer<MessageResponseDto>>(await _messageRepository.Create(mes));
                    return resultMessage;
                // Если нет файлов и нет текста сообщения - вернуть ошибку
                case null when string.IsNullOrEmpty(message.Text):
                    resultMessage.ErrorType = ErrorType.BadRequest;
                    return resultMessage;
            }
            
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                
            resultMessage = _mapper.Map<ResultContainer<MessageResponseDto>>(await _messageRepository.Create(mes));
            var resultUpload = 
                await _uploadService.UploadAsync(message.Files, resultMessage.Data.Id);
            
            if (resultUpload.ErrorType.HasValue)
                resultMessage.ErrorType = ErrorType.BadRequest;
            else
                scope.Complete();

            return resultMessage;
        }

        private async Task<ResultContainer<MessageResponseDto>> ValidateMessage(MessageRequestDto messageDto)
        {
            var user = await _userService.FindByIdAsync(messageDto.UserId);
            var room = await _roomService.FindByIdAsync(messageDto.RoomId);
            var result = new ResultContainer<MessageResponseDto>();

            // Если данные валидны
            if (user.Data != null && room.Data != null) 
                return result;
            
            result.ErrorType = ErrorType.BadRequest;
            return result;
        }
    }
}