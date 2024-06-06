using Puppy.Models.Dto.UserDtos;

namespace Puppy.Models.Dto.PostDtos.LikeDtos;

public class LikedPostsDto
{
    public int PostId { get; set; }
    public int UserId { get; set; }
    public GetPostDto Post { get; set; }
    public ShortUserDto User { get; set; }
}