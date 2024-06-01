using AutoMapper;
using Puppy.Models;
using Puppy.Models.Dto.AdminDtos;
using Puppy.Models.Dto.ChatDto;
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
        CreateMap<ChatMessage, SendMessageDto>();
        CreateMap<ChatMessage, ShortMessagesDto>();
        CreateMap<ChatRoom, ShortRoomDtoU1>();
        CreateMap<ChatRoom, ShortRoomDtoU2>();
        CreateMap<ChatRoom, FullRoomDto>();
        CreateMap<ChatRoom, GetRoomDto>();
        CreateMap<ChatRoom, AllRoomsResponseDto>();
    }
}