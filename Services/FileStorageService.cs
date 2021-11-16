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
        
        public async Task<ResultContainer<ICollection<FileResponseDto>>> Upload(IFormFileCollection files)
        {
            ResultContainer<ICollection<FileResponseDto>> result;

            // Если есть файлы с неподдерживаемым типом
            if (files.Any(f => !FileContentType.ContentTypes.Contains(f.ContentType)))
            {
                result = await CreateBadResult(files);
                return result;
            }

            result = await UploadFiles(files);
            return result;
        }

        private async Task<ResultContainer<ICollection<FileResponseDto>>> CreateBadResult(IFormFileCollection files)
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
        
        private async Task<ResultContainer<ICollection<FileResponseDto>>> UploadFiles(IFormFileCollection files)
        {
            var result = new ResultContainer<ICollection<FileResponseDto>>
            {
                Data = new List<FileResponseDto>()
            };
            
            foreach (var file in files)
            {
                var absolutePath = CreateAbsolutePath(file.FileName);
                
                await using var fileStream = File.Create(absolutePath);
                await file.CopyToAsync(fileStream);

                var fileResponse = _mapper.Map<FileResponseDto>(file);
                fileResponse.DateCreated = DateTime.Now;
                fileResponse.ContentType = file.ContentType;
                fileResponse.Path = absolutePath;
                
                result.Data.Add(_mapper.Map<FileResponseDto>(fileResponse));
            }

            return result;
        }

        private string CreateAbsolutePath(string fileName)
        {
            var extension = Path.GetExtension(fileName);
            var absolutePath = _basePath;
            
            switch (extension)
            {
                case ".png":
                case ".jpg":
                    _catalogues.TryGetValue("Images",  out var catalogue);
                    absolutePath = Path.Combine(_basePath, catalogue, Guid.NewGuid() + extension);
                    break;
            }

            return absolutePath;
        }
    }
}