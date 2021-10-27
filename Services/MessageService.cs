using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Transactions;
using AutoMapper;
using Infrastructure.Contracts;
using Infrastructure.Result;
using Models;
using Models.Dtos;
using Models.Dtos.Image;
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
        private readonly IFileStorageService _uploadService;
        private readonly IImageRepository _imageRepository;

        public MessageService
        (
            IMessageRepository repository, 
            IMapper mapper, 
            IUserService userService,
            IRoomService roomService,
            IFileStorageService uploadService,
            IImageRepository imageRepository
        )
        {
            _messageRepository = repository;
            _mapper = mapper;
            _userService = userService;
            _roomService = roomService;
            _uploadService = uploadService;
            _imageRepository = imageRepository;
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

            if (result.ErrorType.HasValue)
                return result;
            
            result.Data.Images = _mapper.Map<ICollection<ImageResponseDto>>
                (await _imageRepository.GetByMessageId(result.Data.Id));
            return result;
        }
        
        public async Task<ResultContainer<MessageResponseDto>> CreateMessageAsync(MessageRequestDto messageDto)
        {
            var resultMessage = await ValidateMessage(messageDto);
            var resultUpload = new ResultContainer<UploadResponseDto>();
            
            // Если сообщение невалидное или не содержит файлов и текста - вернуть ошибку
            if (resultMessage.ErrorType.HasValue || messageDto.Files == null && messageDto.Text == null)
                return resultMessage;
            
            var message = _mapper.Map<Message>(messageDto);
            message.DateCreated = DateTime.Now;

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            
            resultMessage = _mapper.Map<ResultContainer<MessageResponseDto>>(await _messageRepository.Create(message));
            
            // Если есть файлы - загрузить их на сервер
            if (messageDto.Files != null) 
                resultUpload = await _uploadService.UploadAsync(messageDto.Files, resultMessage.Data.Id);
            
            if (resultUpload.ErrorType.HasValue)
                resultMessage.ErrorType = ErrorType.BadRequest;
            else
            {
                resultMessage.Data.Images = resultUpload.Data.Images;
                scope.Complete();
            }
            
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