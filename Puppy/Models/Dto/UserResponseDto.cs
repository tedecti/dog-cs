using Curs.Models;

namespace Puppy.Models.Dto;

public class UserResponseDto
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
		
    public ICollection<Pet> Pets { get; set; } = new List<Pet>();

    public ICollection<FollowerResponseDto> Friends { get; set; } = new List<FollowerResponseDto>();
    public ICollection<FollowerResponseDto> Followers { get; set; } = new List<FollowerResponseDto>();
    public ICollection<Post> Posts { get; set; } = new List<Post>();
}