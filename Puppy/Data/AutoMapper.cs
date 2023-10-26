using AutoMapper;
using Curs.Models;
using Puppy.Models.Dto;

namespace Curs.Data;

public class AutoMapper : Profile
{
    public AutoMapper()
    {
        CreateMap<User, UserResponseDto>().ForMember(dest=>dest.Pets, opt=>opt.MapFrom(src=>src.Pets));
        CreateMap<User, ShortUserDto>();
        CreateMap<Friend, FollowerResponseDto>();
        CreateMap<Pet, UserPetDto>();
        CreateMap<Pet, GetPetDto>();
        CreateMap<Post, GetPostDto>();
        CreateMap<Commentary, GetCommentsDto>();
    }
}