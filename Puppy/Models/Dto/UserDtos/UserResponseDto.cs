using Curs.Models;

namespace Puppy.Models.Dto;

public class UserResponseDto
{
    public int Id { get; set; }
    public string Avatar { get; set; }
    public string Username { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int FriendsCount { get; set; }
    public int FollowersCount { get; set; }
		
    public ICollection<UserPetDto> Pets { get; set; } = new List<UserPetDto>();

    public ICollection<GetFollowersDto> Friends { get; set; } = new List<GetFollowersDto>();
    public ICollection<GetFollowersDto> Followers { get; set; } = new List<GetFollowersDto>();
    public ICollection<Post> Posts { get; set; } = new List<Post>();
}