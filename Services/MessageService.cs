using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Transactions;
using AutoMapper;
using Infrastructure.Contracts;
using Infrastructure.Result;
using Models;
using Models.Dtos.Image;
using Models.Dtos.Message;
using Models.Dtos.Upload;
using Models.Error;
using Services.Contracts;

namespace Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IMapper _mapper;
        private readonly IFileStorageService _uploadService;
        private readonly IImageRepository _imageRepository;
        private readonly IUserChatService _userChatService;

        public MessageService
        (
            IMessageRepository repository, 
            IMapper mapper,
            IFileStorageService uploadService,
            IImageRepository imageRepository,
            IUserChatService userChatService
        )
        {
            _messageRepository = repository;
            _mapper = mapper;
            _uploadService = uploadService;
            _imageRepository = imageRepository;
            _userChatService = userChatService;
        }

        public async Task<ResultContainer<MessageResponseDto>> GetById(int id)
        {
            var result = new ResultContainer<MessageResponseDto>();
            var message = await _messageRepository.GetById(id);

            if (message == null)
            {
                result.ErrorType = ErrorType.NotFound;
                return result;
            }

            result = _mapper.Map<ResultContainer<MessageResponseDto>>(message);

            result.Data.Images = _mapper.Map<ICollection<ImageResponseDto>>
                (await _imageRepository.GetByMessageId(result.Data.Id));
            return result;
        }
        
        public async Task<ResultContainer<int?>> CountMessagesByChatId(int chatId)
        {
            var result = _mapper.Map<ResultContainer<int?>>(await _messageRepository.Count(chatId));

            if (result.Data != null)
                return result;

            // Если чат не найден
            result.ErrorType = ErrorType.NotFound;
            return result;
        }
        
        public async Task<ResultContainer<MessageResponseDto>> Create(MessageRequestDto messageDto)
        {
            var resultMessage = await CheckUserInChat(messageDto);
            var resultUpload = new ResultContainer<UploadFilesResponseDto>();

            // Если пользователь не состоит в чате
            if (resultMessage.ErrorType.HasValue)
                return resultMessage;

            // Если пользователь не отправил файлов и нет текста сообщения
            if (messageDto.Text == null && messageDto.Files == null)
                return resultMessage;
            
            var message = _mapper.Map<Message>(messageDto);
            message.DateCreated = DateTime.Now;

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            
            resultMessage = _mapper.Map<ResultContainer<MessageResponseDto>>(await _messageRepository.Create(message));

            // Если есть файлы - загрузить их на сервер
            if (messageDto.Files != null) 
                resultUpload = await _uploadService.Upload(messageDto.Files, resultMessage.Data.Id);

            // Если файлы загружены
            if (resultUpload.Data != null)
            {
                resultMessage.Data.Images = resultUpload.Data.Images;
            }

            if (resultUpload.ErrorType.HasValue)
                resultMessage.ErrorType = ErrorType.UnprocessableEntity;
            else
                scope.Complete();

            return resultMessage;
        }

        private async Task<ResultContainer<MessageResponseDto>> CheckUserInChat(MessageRequestDto messageDto)
        {
            var result = new ResultContainer<MessageResponseDto>();
            var userInChat = 
                await _userChatService.CheckUserInChat(messageDto.UserId, messageDto.ChatId);
            
            if (!userInChat.ErrorType.HasValue) 
                return result;
            
            result.ErrorType = ErrorType.NotFound;
            return result;
        }
    }
}