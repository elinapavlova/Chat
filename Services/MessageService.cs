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
        
        public async Task<ResultContainer<MessageResponseDto>> CreateMessageAsync(MessageResponseDto messageDto)
        {
            var user = await _userService.FindByIdAsync(messageDto.UserId);
            var room = await _roomService.FindByIdAsync(messageDto.RoomId);
            var result = new ResultContainer<MessageResponseDto>();
            
            if (user.Data == null || room.Data == null || messageDto.Text == null)
            {
                result.ErrorType = ErrorType.BadRequest;
                return result;
            }
            
            var newMessage = _mapper.Map<MessageResponseDto, Message>(messageDto);
            newMessage.DateCreated = DateTime.Now;
            
            result = _mapper.Map<ResultContainer<MessageResponseDto>>(await _messageRepository.Create(newMessage));
            return result;
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
        
        public async Task<ResultContainer<MessageResponseDto>> UploadMessageAsync(MessageRequestDto message)
        {
            var resultMessage = new ResultContainer<MessageResponseDto>();
            var mes = _mapper.Map<Message>(message);
            mes.DateCreated = DateTime.Now;
            
            if (message.Files == null && !string.IsNullOrEmpty(message.Text))
            {
                resultMessage = _mapper.Map<ResultContainer<MessageResponseDto>>(await _messageRepository.Create(mes));
            }
            else if (message.Files != null)
            {
                using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                
                resultMessage = _mapper.Map<ResultContainer<MessageResponseDto>>(await _messageRepository.Create(mes));
                await UploadImage(message.Files, resultMessage.Data.Id);

                scope.Complete();
            }
            else
            {
                resultMessage.ErrorType = ErrorType.BadRequest;
                return resultMessage;
            }
            
            return resultMessage;
        }

        private async Task UploadImage(IFormFileCollection imageFile, int messageId)
        {
            var image = new Image();
            var result = new ResultContainer<ImageResponseDto>(); 
            byte[] imageBytes;

            foreach (var file in imageFile)
            {
                if (file.ContentType != "image/jpeg")
                {
                    result.ErrorType = ErrorType.BadRequest;
                    return;
                }
                            
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
        }
    }
}