using AutoMapper;
using Models;
using Models.Base;
using Models.Dtos;
using Models.Dtos.Room;
using Models.Dtos.User;

namespace Infrastructure.Profiles
{
    public class DtoToModelProfile : Profile
    {
        public DtoToModelProfile()
        {
            CreateMap<UserCredentialsDto, User>();

            CreateMap<RoomDto, Room>();

            CreateMap<MessageResponseDto, Message>();
        }
    }
}