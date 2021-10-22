using System;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.Contracts;
using Infrastructure.Result;
using Microsoft.AspNetCore.Http;
using Models.Dtos;
using Models.Dtos.Image;
using Models.Error;
using Services.Contracts;

namespace Services
{
    public class UploadService  : IUploadService
    {
        private readonly IImageRepository _imageRepository;
        private readonly IMapper _mapper;

        public UploadService
        (
            IMapper mapper,
            IImageRepository imageRepository
        )
        {
            _mapper = mapper;
            _imageRepository = imageRepository;
        }

        public async Task<ResultContainer<UploadResponseDto>> UploadAsync(IFormFileCollection files, int messageId)
        {
            var result = new ResultContainer<UploadResponseDto>();
            foreach (var file in files)
            {
                switch (file.ContentType)
                {
                    case "image/jpeg" : 
                        var res = await UploadImageAsync(file, messageId);
                        if (res.ErrorType.HasValue)
                        {
                            result.ErrorType = ErrorType.BadRequest;
                            return result;
                        }
                        break;
                    default:
                        result.ErrorType = ErrorType.BadRequest;
                        return result;
                }
            }
            
            return result;
        }

        private async Task<ResultContainer<ImageResponseDto>> UploadImageAsync(IFormFile file, int messageId)
        {
            var image = new ImageDto();
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
                            
            var result = _mapper.Map<ResultContainer<ImageResponseDto>>(await _imageRepository.Create(image));
            return result;
        }
    }
}