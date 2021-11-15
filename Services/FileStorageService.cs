using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.Error;
using Infrastructure.File;
using Infrastructure.Options;
using Infrastructure.Result;
using Microsoft.AspNetCore.Http;
using Models.FileModel;
using Models.UploadModel;

namespace Services
{
    public class FileStorageService : IFileStorageService
    {
        private readonly string _basePath;
        private readonly IMapper _mapper;
        private readonly Dictionary<string, string> _catalogues;

        public FileStorageService
        (
            FileStorageOptions options,
            IMapper mapper
        )
        {
            _basePath = options.BasePath;
            _catalogues = options.CataloguesName;
            _mapper = mapper;
        }
        
        public async Task<ResultContainer<ICollection<FileResponseDto>>> Upload(UploadRequestDto files)
        {
            ResultContainer<ICollection<FileResponseDto>> result;
            
            // Если есть файлы с неподдерживаемым типом
            if (!files.Files.All(f => FileContentType.ContentTypes.Contains(f.ContentType)))
            {
                result = await ConfigureBadResult(files.Files);
                return result;
            }

            result = await UploadFiles(files);
            return result;
        }

        private async Task<ResultContainer<ICollection<FileResponseDto>>> ConfigureBadResult(IEnumerable<FileRequestDto> files)
        {
            var result = new ResultContainer<ICollection<FileResponseDto>>
            {
                Data = new List<FileResponseDto>()
            };
            
            foreach (var file in files)
            {
                // Если у файла неподдерживаемый тип
                if (!FileContentType.ContentTypes.Contains(file.ContentType))
                {
                    var invalidFile = _mapper.Map<FileResponseDto>(file);
                    result.Data.Add(invalidFile);
                    break;
                }
                
                result.Data.Add(_mapper.Map<FileResponseDto>(file));
            }
            
            result.ErrorType = ErrorType.InvalidFileExtension;
            
            return result;
        }
        
        private async Task<ResultContainer<ICollection<FileResponseDto>>> UploadFiles(UploadRequestDto files)
        {
            var result = new ResultContainer<ICollection<FileResponseDto>>
            {
                Data = new List<FileResponseDto>()
            };
            
            foreach (var file in files.Files)
            {
                await using var stream = new MemoryStream(file.File);
                
                var formFile = ConvertByteArrayToFormFile(stream, file);
                var absoletePath = ConfigureAbsoletePath(file.Name);
                
                await using var fileStream = File.Create(absoletePath);
                await formFile.CopyToAsync(fileStream);

                var fileResponse = _mapper.Map<FileResponseDto>(file);
                fileResponse.DateCreated = DateTime.Now;
                fileResponse.MessageId = files.MessageId;
                fileResponse.ContentType = file.ContentType;
                fileResponse.Path = absoletePath;
                
                result.Data.Add(_mapper.Map<FileResponseDto>(fileResponse));
            }

            return result;
        }

        private static FormFile ConvertByteArrayToFormFile(Stream stream, FileRequestDto file)
        {
            var formFile = new FormFile(stream, 0, file.File.Length, "file", file.Name)
            {
                Headers = new HeaderDictionary(),
                ContentType = file.ContentType
            };

            return formFile;
        }

        private string ConfigureAbsoletePath(string fileName)
        {
            var extension = fileName.Split('.').LastOrDefault();
            var absoletePath = _basePath;
            
            switch (extension)
            {
                case "jpg":
                    _catalogues.TryGetValue("Images",  out var catalogue);
                    absoletePath += catalogue + Guid.NewGuid() + "." + extension;
                    break;
            }

            return absoletePath;
        }
    }
}