using Puppy.Models.Dto.PostDtos;
using Puppy.Models.Dto.UserDtos;

namespace Puppy.Models.Dto.ComplaintDtos;

public class GetComplaintDto
{
    public int Id { get; set; }
    public string Commentary { get; set; }
    public string Status { get; set; }
    public ShortUserDto User { get; set; }
    public GetPostDto Post { get; set; }
}