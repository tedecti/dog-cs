namespace Puppy.Models.Dto.ComplaintDtos;

public class ShortComplaintDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int PostId { get; set; }
    public string Commentary { get; set; }
    public string Status { get; set; }
}