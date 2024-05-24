using Puppy.Models.Dto.UserDtos;

namespace Puppy.Models.Dto.ComplaintDtos;

public class ComplaintResponseDto
{
    public int PostId { get; set; }
    public ShortUserDto User { get; set; }
    public string Commentary { get; set; }
}