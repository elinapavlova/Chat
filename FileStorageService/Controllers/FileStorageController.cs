﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Result;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.FileModel;
using Services;

namespace FileStorageService.Controllers
{
    [ApiVersion("1.0")]
    [Route("/api/[controller]")]
    [ApiController]
    public class FileStorageController : ControllerBase
    {
        private readonly IFileStorageService _fileStorageService;
        
        public FileStorageController(IFileStorageService fileStorageService)
        {
            _fileStorageService = fileStorageService;
        }
        
        [HttpPost("Upload")]
        public async Task<ResultContainer<ICollection<FileResponseDto>>> Upload(IFormFileCollection files)
        {
            var result = await _fileStorageService.Upload(files);
            return result;
        }
    }
}