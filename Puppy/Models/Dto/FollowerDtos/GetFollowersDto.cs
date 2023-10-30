namespace Puppy.Models.Dto;

public class GetFollowersDto
{
    public ShortUserDto User { get; set; }
    public ShortUserDto Follower { get; set; }
}