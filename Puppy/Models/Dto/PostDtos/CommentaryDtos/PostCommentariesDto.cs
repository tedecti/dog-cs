namespace Puppy.Models.Dto.PostDtos.CommentaryDtos;

public class PostCommentariesDto
{
    public int Total { get; set; }
    public IEnumerable<GetCommentsDto> Comments { get; set; }
}