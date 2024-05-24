using Puppy.Models.Dto.ComplaintDtos;

namespace Puppy.Models.Dto.UserDtos;

public class UserComplaintsDto
{
    public int Id { get; set; }
    public string Avatar { get; set; }
    public string Username { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public ICollection<ShortComplaintDto> Complaints { get; set; }
}