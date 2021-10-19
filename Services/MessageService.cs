using System;
using System.IO;
using System.Threading.Tasks;
using System.Transactions;
using AutoMapper;
using Infrastructure.Contracts;
using Infrastructure.Result;
using Microsoft.AspNetCore.Http;
using Models;
using Models.Dtos;
using Models.Dtos.Message;
using Models.Error;
using Services.Contracts;

namespace Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IImageRepository _imageRepository;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IRoomService _roomService;
        
        public MessageService
        (
            IMessageRepository repository, 
            IMapper mapper, 
            IUserService userService,
            IRoomService roomService,
            IImageRepository imageRepository
        )
        {
            _messageRepository = repository;
            _mapper = mapper;
            _userService = userService;
            _roomService = roomService;
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
            var resultImage = await UploadImage(message.Files, resultMessage.Data.Id);

            if (resultImage.ErrorType.HasValue)
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
        
        private async Task<ResultContainer<ImageResponseDto>> UploadImage(IFormFileCollection files, int messageId)
        {
            var image = new Image();
            var result = new ResultContainer<ImageResponseDto>();

            foreach (var file in files)
            {
                if (file.ContentType != "image/jpeg")
                {
                    result.ErrorType = ErrorType.BadRequest;
                    return result;
                }

                byte[] imageBytes;
                await using (var stream = file.OpenReadStream())
                await using (var memoryStream = new MemoryStream())
                {
                    await stream.CopyToAsync(memoryStream);
                    imageBytes = memoryStream.ToArray();
                }
                
                image.Img = imageBytes;
                image.DateCreated = DateTime.Now;
                image.MessageId = messageId;
                            
                result = _mapper.Map<ResultContainer<ImageResponseDto>>(await _imageRepository.Create(image));
            }

            return result;
        }
    }
}