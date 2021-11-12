using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.Contracts;
using Infrastructure.Result;
using Microsoft.AspNetCore.Http;
using Models.Dtos.File;
using Models.Dtos.Image;
using Models.Dtos.Upload;
using Models.Error;
using Newtonsoft.Json;
using Services.Contracts;

namespace Services
{
    public class FileStorageService  : IFileStorageService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IMapper _mapper;
        private readonly IImageRepository _imageRepository;

        public FileStorageService
        (
            IHttpClientFactory clientFactory,
            IMapper mapper,
            IImageRepository imageRepository
        )
        {
            _clientFactory = clientFactory;
            _mapper = mapper;
            _imageRepository = imageRepository;
        }

        public async Task<ResultContainer<UploadFilesResponseDto>> Upload(IFormFileCollection files, int messageId)
        {
            var result = new ResultContainer<UploadFilesResponseDto>();
            var content = await ConfigureContent(files, messageId);

            if (content.ErrorType.HasValue)
            {
                result.ErrorType = ErrorType.BadRequest;
                return result;
            }

            using var client = _clientFactory.CreateClient("FileStorage");
            var response = await client.PostAsJsonAsync("Upload", content.Data);
            var responseMessage = await response.Content.ReadAsStringAsync();

            var responseJson = JsonConvert.DeserializeObject<UploadResponseDto>(responseMessage); 
            result = await ValidateResult(responseJson);

            return result;
        }

        private async Task<ResultContainer<UploadRequestDto>> ConfigureContent(IFormFileCollection files, int messageId)
        {
            var content = new UploadRequestDto
            {
                MessageId = messageId,
                Files = new List<FileRequestDto>()
            };

            var result = new ResultContainer<UploadRequestDto>();

            foreach (var file in files)
            {
                // Если файл пустой
                if (file.Length <= 0)
                {
                    result.ErrorType = ErrorType.BadRequest;
                    return result;
                }
                
                var stream = new MemoryStream((int) file.Length);
                await file.CopyToAsync(stream);

                content.Files.Add(new FileRequestDto
                {
                    File = stream.ToArray(),
                    Name = file.FileName,
                    ContentType = file.ContentType
                });
            }

            result = _mapper.Map<ResultContainer<UploadRequestDto>>(content);
            return result;
        }

        private async Task<ResultContainer<UploadFilesResponseDto>> ValidateResult(UploadResponseDto files)
        {
            var result = new ResultContainer<UploadFilesResponseDto>
            {
                Data = new UploadFilesResponseDto
                {
                    Images = new List<ImageResponseDto>()
                }
            };

            if (files.ErrorType != null)
            {
                result.ErrorType = ErrorType.UnprocessableEntity;
                return result;
            }
            
            foreach (var file in files.Data)
            {
                switch (file.ContentType)
                {
                    case "image/jpeg" :
                        var res = await AddImageToDatabase(file);
                        result.Data.Images.Add(res);
                        break;
                }
            }

            return result;
        }

        private async Task<ImageResponseDto> AddImageToDatabase(FileResponseDto file)
        {
            var image = _mapper.Map<ImageDto>(file); 
            var result = _mapper.Map<ImageResponseDto>(await _imageRepository.Create(image));
            
            return result;
        }
    }
}