using System.Collections.Generic;
using AutoMapper;
using Infrastructure.Result;
using Models;
using Models.Dtos.Chat;
using Models.Dtos.File;
using Models.Dtos.Image;
using Models.Dtos.Message;
using Models.Dtos.Room;
using Models.Dtos.Token;
using Models.Dtos.Upload;
using Models.Dtos.User;
using Models.Dtos.UserChat;
using Models.Dtos.UserRoom;
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
            CreateMap<MessageRequestDto, Message>();
            CreateMap<ImageResponseDto, Image>();
            CreateMap<ChatDto, Chat>();
            CreateMap<UserRoomDto, UserRoom>();
            CreateMap<UserRoomResponseDto, UserRoom>();
            CreateMap<UserChatDto, UserChat>();
            CreateMap<UserChatResponseDto, UserChat>();
            
            // Model to Dto
            CreateMap<User, UserDto>();
            CreateMap<User, UserCredentialsDto>();
            CreateMap<Room, RoomDto>();
            CreateMap<Room, RoomResponseDto>();
            CreateMap<Message, MessageResponseDto>();
            CreateMap<Image, ImageResponseDto>();
            CreateMap<Chat, ChatDto>();
            CreateMap<Chat, ChatResponseDto>();
            CreateMap<UserRoom, UserRoomDto>();
            CreateMap<UserRoom, UserRoomResponseDto>();
            CreateMap<UserChat, UserChatDto>();
            CreateMap<UserChat, UserChatResponseDto>();
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
            CreateMap<UserRoom, ResultContainer<UserRoomDto>>()
                .ForMember("Data", opt => 
                    opt.MapFrom(u => u));
            
            CreateMap<UserRoom, ResultContainer<UserRoomResponseDto>>()
                .ForMember("Data", opt => 
                    opt.MapFrom(u => u));
            
            CreateMap<ICollection<UserRoom>, ResultContainer<ICollection<UserRoomDto>>>()
                .ForMember("Data", opt =>
                    opt.MapFrom(r => r));
            
            CreateMap<UserChat, ResultContainer<UserChatDto>>()
                .ForMember("Data", opt => 
                    opt.MapFrom(u => u));
            
            CreateMap<UserChat, ResultContainer<UserChatResponseDto>>()
                .ForMember("Data", opt => 
                    opt.MapFrom(u => u));
            
            CreateMap<ICollection<UserChat>, ResultContainer<ICollection<UserChatDto>>>()
                .ForMember("Data", opt =>
                    opt.MapFrom(r => r));
            
            CreateMap<User, ResultContainer<UserDto>>()
                .ForMember("Data", opt => 
                    opt.MapFrom(u => u));
            
            CreateMap<User, ResultContainer<UserCredentialsDto>>()
                .ForMember("Data", opt =>
                    opt.MapFrom(r => r));
            
            CreateMap<ICollection<User>, ResultContainer<ICollection<UserDto>>>()
                .ForMember("Data", opt =>
                    opt.MapFrom(r => r));

            CreateMap<Message, ResultContainer<MessageResponseDto>>()
                .ForMember("Data", opt =>
                    opt.MapFrom(m => m));
            
            CreateMap<ICollection<MessageResponseDto>, ResultContainer<ICollection<MessageResponseDto>>>()
                .ForMember("Data", opt =>
                    opt.MapFrom(c => c));
            
            CreateMap<ICollection<Message>, ResultContainer<ICollection<MessageResponseDto>>>()
                .ForMember("Data", opt =>
                    opt.MapFrom(r => r));

            CreateMap<Room, ResultContainer<RoomDto>>()
                .ForMember("Data", opt =>
                    opt.MapFrom(r => r));

            CreateMap<Room, ResultContainer<RoomResponseDto>>()
                .ForMember("Data", opt =>
                    opt.MapFrom(r => r));
            
            CreateMap<ICollection<Room>, ResultContainer<ICollection<RoomDto>>>()
                .ForMember("Data", opt =>
                    opt.MapFrom(r => r));

            CreateMap<AccessToken, ResultContainer<AccessTokenDto>>()
                .ForMember("Data", opt =>
                    opt.MapFrom(r => r));

            CreateMap<Image, ResultContainer<ImageResponseDto>>()
                .ForMember("Data", opt =>
                    opt.MapFrom(r => r));

            CreateMap<Chat, ResultContainer<ChatResponseDto>>()
                .ForMember("Data", opt =>
                    opt.MapFrom(r => r));

            CreateMap<Chat, ResultContainer<ChatDto>>()
                .ForMember("Data", opt =>
                    opt.MapFrom(c => c));

            CreateMap<ICollection<Chat>, ResultContainer<ICollection<ChatDto>>>()
                .ForMember("Data", opt =>
                    opt.MapFrom(c => c));
            
            CreateMap<int, ResultContainer<int>>()
                .ForMember("Data", opt =>
                    opt.MapFrom(c => c));

            CreateMap<FileResponseDto, ImageDto>();
        }
    }
}