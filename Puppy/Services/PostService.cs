using Curs.Models;
using Microsoft.EntityFrameworkCore;
using Puppy.Data;
using Puppy.Models.Dto;
using Puppy.Repository;

namespace Puppy.Services;

public class PostService : IPostService
{
    private readonly AppDbContext _context;

    public PostService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Post>> GetPosts(int userId)
    {
        var friends = await _context.Friend.Where(f => f.UserId == userId).ToListAsync();

        if (friends.Any())
        {
            var friendIds = friends.Select(f => f.FollowerId).ToList();

            var friendPosts = await _context.Post
                .Where(p => friendIds.Contains(p.UserId))
                .Include(p=>p.User)
                .ToListAsync();

            var otherPosts = await _context.Post
                .Where(p => !friendIds.Contains(p.UserId))
                .Include(p=>p.User)
                .OrderByDescending(p => p.UploadDate)
                .ToListAsync();

            var combinedPosts = friendPosts.Concat(otherPosts);
        }

        var allPosts = await _context.Post
            .Include(p => p.User)
            .OrderByDescending(p => p.UploadDate)
            .ToListAsync();
        return allPosts;
    }

    public async Task<Post> GetPostById(int postId)
    {
        var post = await _context.Post.Include(p => p.User).FirstOrDefaultAsync(p=> p.Id == postId);
        return post;
    }
}