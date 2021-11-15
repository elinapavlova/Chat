using AutoMapper;
using Models.FileModel;

namespace Infrastructure.Profiles
{
    public class AppProfile : Profile
    {
        public AppProfile()
        {
            CreateMap<FileRequestDto, FileResponseDto>();
        }
    }
}