using System.Collections.Generic;
using AutoMapper;
using Infrastructure.Result;
using Models;
using Models.Dtos;
using Models.Dtos.Image;
using Models.Dtos.Message;
using Models.Dtos.Room;
using Models.Dtos.Token;
using Models.Dtos.User;
using Models.Token;

namespace Infrastructure.Profiles
{
    public class AppProfile : Profile
    {
        public AppProfile()
        {
            // Dto to Model
            CreateMap<UserCredentialsDto, User>();
            CreateMap<RoomDto, Room>();
            CreateMap<MessageResponseDto, Message>();
            CreateMap<ImageResponseDto, Image>();
            CreateMap<MessageRequestDto, Message>();
            
            // Model to Dto
            CreateMap<User, UserDto>();
            CreateMap<User, UserCredentialsDto>();
            CreateMap<Room, RoomDto>();
            CreateMap<Room, RoomResponseDto>();
            CreateMap<Message, MessageResponseDto>();
            CreateMap<Image, ImageResponseDto>();
            CreateMap<AccessToken, AccessTokenDto>()
                .ForMember(a => a.AccessToken, 
                    opt => 
                        opt.MapFrom(a => a.Token))
                .ForMember(a => a.RefreshToken, 
                    opt => 
                        opt.MapFrom(a => a.RefreshToken.Token))
                .ForMember(a => a.Expiration, 
                    opt => 
                        opt.MapFrom(a => a.Expiration));
            
            // Model to ResultContainer
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

            CreateMap<Image, ResultContainer<ImageResponseDto>>()
                .ForMember("Data", opt =>
                    opt.MapFrom(r => r));

            CreateMap<UploadResponseDto, ResultContainer<UploadResponseDto>>()
                .ForMember("Data", opt =>
                    opt.MapFrom(r => r));
        }
    }
}