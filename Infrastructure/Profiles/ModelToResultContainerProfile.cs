using System.Collections.Generic;
using AutoMapper;
using Infrastructure.Result;
using Models;
using Models.Base;
using Models.Dtos;
using Models.Dtos.Room;
using Models.Dtos.Token;
using Models.Dtos.User;
using Models.Token;

namespace Infrastructure.Profiles
{
    public class ModelToResultContainerProfile : Profile
    {
        public ModelToResultContainerProfile()
        {
            CreateMap<User, ResultContainer<UserDto>>()
                .ForMember("Data", opt => 
                    opt.MapFrom(u => u));

            CreateMap<Message, ResultContainer<MessageResponseDto>>()
                .ForMember("Data", opt =>
                    opt.MapFrom(m => m));

            CreateMap<Room, ResultContainer<RoomDto>>()
                .ForMember("Data", opt =>
                    opt.MapFrom(r => r));
            
            CreateMap<Room, ResultContainer<RoomResponseDto>>()
                .ForMember("Data", opt =>
                    opt.MapFrom(r => r));
            
            CreateMap<User, ResultContainer<UserCredentialsDto>>()
                .ForMember("Data", opt =>
                    opt.MapFrom(r => r));
            
            CreateMap<AccessToken, ResultContainer<AccessTokenDto>>()
                .ForMember("Data", opt =>
                    opt.MapFrom(r => r));
            
            CreateMap<ICollection<User>, ResultContainer<ICollection<UserDto>>>()
                .ForMember("Data", opt =>
                    opt.MapFrom(r => r));
            
            CreateMap<ICollection<Message>, ResultContainer<ICollection<MessageResponseDto>>>()
                .ForMember("Data", opt =>
                    opt.MapFrom(r => r));
            
            CreateMap<ICollection<Room>, ResultContainer<ICollection<RoomDto>>>()
                .ForMember("Data", opt =>
                    opt.MapFrom(r => r));
        }
    }
}