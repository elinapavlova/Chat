using AutoMapper;
using Models;
using Models.Base;
using Models.Dtos;
using Models.Dtos.Room;
using Models.Dtos.Token;
using Models.Dtos.User;
using Models.Token;

namespace Infrastructure.Profiles
{
    public class ModelToDtoProfile : Profile
    {
        public ModelToDtoProfile()
        {
            CreateMap<User, UserDto>();

            CreateMap<User, UserCredentialsDto>();

            CreateMap<Room, RoomDto>();

            CreateMap<Room, RoomResponseDto>();

            CreateMap<Message, MessageResponseDto>();
            
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

        }
    }
}