namespace Puppy.Models.Dto;

public class PostCommentaries
{
    public int Total { get; set; }
    public IEnumerable<GetCommentsDto> Comments { get; set; }
}