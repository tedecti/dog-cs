using Puppy.Models.Dto.UserDtos;

namespace Puppy.Models.Dto.FollowerDtos;

public class GetFollowersDto
{
    public ShortUserDto User { get; set; }
    public ShortUserDto Follower { get; set; }
}