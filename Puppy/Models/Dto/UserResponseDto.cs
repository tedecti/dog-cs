using Curs.Models;

namespace Puppy.Models.Dto;

public class UserResponseDto
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
		
    public ICollection<UserPetDto> Pets { get; set; } = new List<UserPetDto>();

    public ICollection<FollowerResponseDto> Friends { get; set; } = new List<FollowerResponseDto>();
    public ICollection<FollowerResponseDto> Followers { get; set; } = new List<FollowerResponseDto>();
    public ICollection<Post> Posts { get; set; } = new List<Post>();
}