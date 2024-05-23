using AutoMapper;
using Puppy.Models;
using Puppy.Models.Dto.AdminDtos;
using Puppy.Models.Dto.ComplaintDtos;
using Puppy.Models.Dto.DocumentDto;
using Puppy.Models.Dto.FollowerDtos;
using Puppy.Models.Dto.PetDtos;
using Puppy.Models.Dto.PostDtos;
using Puppy.Models.Dto.PostDtos.CommentaryDtos;
using Puppy.Models.Dto.UserDtos;

namespace Puppy.Data;

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
        CreateMap<Pet, ShortPetDto>();
        CreateMap<Post, GetPostDto>();
        CreateMap<Commentary, GetCommentsDto>();
        CreateMap<Document, GetDocumentDto>();
        CreateMap<Document, ShortDocumentDto>();
        CreateMap<Complaint, UserComplaintsDto>();
        CreateMap<User, UserComplaintsDto>();
        CreateMap<Post, PostComplaintsDto>();
        CreateMap<Complaint, ShortComplaintDto>();
        CreateMap<Complaint, GetComplaintDto>();
        CreateMap<Admin, AdminDto>();
    }
}