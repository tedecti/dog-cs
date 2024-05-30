using Puppy.Models;
using Puppy.Models.Dto.ComplaintDtos;
using Puppy.Models.Dto.UserDtos;

namespace Puppy.Repositories.Interfaces;

public interface IComplaintRepository
{
    Task<Complaint> CreateComplaint(ComplaintRequestDto complaintRequestDto, int UserId, int PostId);
    Task <IEnumerable<Complaint>> GetUserComplaints(int UserId);
    Task<IEnumerable<User>?> GetUsers();
    Task<Post> GetPostComplaints(int PostId);
    Task<Complaint> SetStatus(ComplaintEditDto complaintEditDto, int ComplaintId);
    Task<Complaint> GetComplaint(int id);
    Task<IEnumerable<Complaint>> GetComplaints();
}