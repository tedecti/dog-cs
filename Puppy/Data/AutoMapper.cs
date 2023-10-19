using AutoMapper;
using Curs.Models;
using Puppy.Models.Dto;

namespace Curs.Data;

public class AutoMapper : Profile
{
    public AutoMapper()
    {
        CreateMap<User, UserResponseDto>().ForMember(dest=>dest.Pets, opt=>opt.MapFrom(src=>src.Pets));
        CreateMap<Friend, FollowerResponseDto>();
        CreateMap<Pet, UserPetDto>();
    }
}