namespace Puppy.Models.Dto;

public class GetCommentsDto
{
    public int PostId { get; set; }
    public ShortUserDto User { get; set; }
    public string Text { get; set; }
}