using Puppy.Models.Dto.UserDtos;

namespace Puppy.Models.Dto.PostDtos.CommentaryDtos;

public class GetCommentsDto
{
    public int PostId { get; set; }
    public ShortUserDto User { get; set; }
    public string Text { get; set; }
}