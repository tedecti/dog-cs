using AutoMapper;
using Curs.Models;
using Curs.Models.Dto.DocumentDto;
using Puppy.Models.Dto;

namespace Curs.Data;

public class AutoMapper : Profile
{
    public AutoMapper()
    {
        CreateMap<User, UserResponseDto>();
            // .ForMember(dest => dest.Pets, opt => opt.MapFrom(src => src.Pets));
            // .ForMember(dest=>dest.FriendsCount, opt=>opt.MapFrom(x=>x.Friends.Count));
        CreateMap<User, ShortUserDto>();
        CreateMap<Friend, GetFollowersDto>();
        CreateMap<Friend, FollowerResponseDto>();
        CreateMap<Pet, UserPetDto>();
        CreateMap<Pet, GetPetDto>();
        CreateMap<Post, GetPostDto>();
        CreateMap<Commentary, GetCommentsDto>();
        CreateMap<Document, GetDocumentDto>();
    }
}