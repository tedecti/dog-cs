using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Puppy.Data;
using Puppy.Models;
using Puppy.Models.Dto.ComplaintDtos;
using Puppy.Repositories.Interfaces;

namespace Puppy.Repositories;

public class ComplaintRepository : IComplaintRepository
{
    private readonly AppDbContext _context;

    public ComplaintRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task<Complaint> CreateComplaint(ComplaintRequestDto complaintRequestDto, int UserId, int PostId)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == UserId);
        var post = await _context.Post.FirstOrDefaultAsync(x => x.Id == PostId);
        if (user == null || post == null)
        {
            return null;
        }
        var complaint = new Complaint()
        {
            UserId = UserId,
            PostId = PostId,
            Commentary = complaintRequestDto.Commentary,
            Status = "New",
            UploadDate = DateTime.UtcNow
        };
        _context.Complaint.Add(complaint);
        await _context.SaveChangesAsync();
        return complaint;
    }

    public async Task<User> GetUserComplaints(int userId)
    {
        var user = await _context.Users
            .Include(x=> x.Complaints.Where(x=> x.Status != "CLosed"))
            .FirstOrDefaultAsync(x => x.Id == userId);
        if (user == null)
        {
            return null;
        }
        return user;
    }
    
    public async Task<Post> GetPostComplaints(int postId)
    {
        var post = await _context.Post
            .Include(x=> x.User)
            .Include(x=> x.Complaints.Where(x=>x.Status != "Closed"))
            .FirstOrDefaultAsync(x => x.Id == postId);
        if (post == null)
        {
            return null;
        }
        return post;
    }

    public async Task<IEnumerable<User>?> GetUsers()
    {
        var users = await _context.Users
            .Include(x => x.Complaints.Where(x=> x.Status != "Closed"))
            .ToListAsync();
        return users;
    }

    public async Task<Complaint> SetStatus(ComplaintEditDto complaintEditDto, int complaintId)
    {
        var complaint = await _context.Complaint.FirstOrDefaultAsync(x => x.Id == complaintId);
        if (complaint == null)
        {
            return null;
        }
        
        complaint.Status = complaintEditDto.Status;
        await _context.SaveChangesAsync();
        return complaint;
    }

    public async Task<Complaint> GetComplaint(int id)
    {
        var complaint = await _context.Complaint
            .Where(x=> x.Status != "Closed")
            .FirstOrDefaultAsync(x => x.Id == id);
        if (complaint == null)
        {
            return null;
        }
        return complaint;
    }
    public async Task<IEnumerable<Complaint>> GetComplaints()
    {
        var complaint = await _context.Complaint
            .Include(x=> x.User)
            .Include(x=> x.Post)
            .Where(x=> x.Status != "Closed")
            .OrderByDescending(p => p.UploadDate)
            .ToListAsync();
        if (complaint == null)
        {
            return null;
        }
        return complaint;
    }
}