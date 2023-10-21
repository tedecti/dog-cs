namespace Puppy.Models.Dto;

public class GetCommentsDto
{
    public int PostId { get; set; }
    public int UserId { get; set; }
    public string Text { get; set; }
}