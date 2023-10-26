namespace Puppy.Models.Dto;

public class PostCommentaries
{
    public int total { get; set; }
    public IEnumerable<GetCommentsDto> comments { get; set; }
}