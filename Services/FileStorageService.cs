using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
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
            var content = await CreateContent(files);
            var result = new ResultContainer<UploadFilesResponseDto>();
            
            if (content == null)
            {
                result.ErrorType = ErrorType.BadRequest;
                return result;
            }
            
            using var client = _clientFactory.CreateClient("FileStorage");
            var response = await client.PostAsync("Upload", content);
            var responseMessage = await response.Content.ReadAsStringAsync();

            var responseJson = JsonConvert.DeserializeObject<UploadResponseDto>(responseMessage); 
            result = await ValidateResult(responseJson, messageId);

            return result;
        }

        private async Task<MultipartFormDataContent> CreateContent(IFormFileCollection files)
        {
            var multiContent = new MultipartFormDataContent();

            if (files.Count > 10)
                return null;
            
            foreach (var file in files)
            {
                if (file.Length <= 0)
                    return null;

                var streamContent = new StreamContent(file.OpenReadStream());
                streamContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
                multiContent.Add(streamContent, file.Name, file.FileName);
            }

            return multiContent;
        }

        private async Task<ResultContainer<UploadFilesResponseDto>> ValidateResult(UploadResponseDto files, int messageId)
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
                result.ErrorType = ErrorType.BadRequest;
                return result;
            }
            
            foreach (var file in files.Data)
            {
                file.MessageId = messageId;
                switch (file.ContentType)
                {
                    case "image/png" :
                    case "image/jpeg":
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