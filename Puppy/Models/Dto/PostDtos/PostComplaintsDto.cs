using Puppy.Models.Dto.ComplaintDtos;
using Puppy.Models.Dto.UserDtos;
namespace Puppy.Models.Dto.PostDtos;

public class PostComplaintsDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string[] Imgs { get; set; }
    public DateTime UploadDate { get; set; }
    public ShortUserDto User { get; set; }
    public ICollection<ShortComplaintDto> Complaints { get; set; }
}