using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.Contracts;
using Infrastructure.Options;
using Infrastructure.Result;
using Microsoft.AspNetCore.Http;
using Models.Dtos;
using Models.Dtos.Image;
using Models.Error;
using Services.Contracts;

namespace Services
{
    public class FileStorageService  : IFileStorageService
    {
        private readonly IImageRepository _imageRepository;
        private readonly IMapper _mapper;
        private readonly string _basePath;

        public FileStorageService
        (
            IMapper mapper,
            IImageRepository imageRepository,
            AppOptions appOptions
        )
        {
            _mapper = mapper;
            _imageRepository = imageRepository;
            _basePath = appOptions.BasePath;
        }

        public async Task<ResultContainer<UploadResponseDto>> UploadAsync(IFormFileCollection files, int messageId)
        {
            var result = new ResultContainer<UploadResponseDto>
            {
                Data = new UploadResponseDto
                {
                    Images = new List<ImageResponseDto>()
                }
            };

            foreach (var file in files)
            {
                switch (file.ContentType)
                {
                    case "image/jpeg" : 
                        var image = await UploadImageAsync(file, messageId);
                        result.Data.Images.Add(image.Data);
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
            var absoletePath = _basePath + file.FileName + ".jpeg";
            if (file.Length > 0)
            {
                await using var stream = File.Create(absoletePath);
                await file.CopyToAsync(stream);
            }
                
            image.Path = absoletePath;
            image.DateCreated = DateTime.Now;
            image.MessageId = messageId;
                            
            var result = _mapper.Map<ResultContainer<ImageResponseDto>>(await _imageRepository.Create(image));
            
            return result;
        }
    }
}