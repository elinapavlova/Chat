using AutoMapper;
using Microsoft.AspNetCore.Http;
using Models.FileModel;

namespace Infrastructure.Profiles
{
    public class AppProfile : Profile
    {
        public AppProfile()
        {
            CreateMap<IFormFile, FileResponseDto>()
                .ForMember("Name", opt => 
                    opt.MapFrom(f => f.FileName));
        }
    }
}