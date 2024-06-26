using Microsoft.EntityFrameworkCore;
using Puppy.Data;
using Puppy.Models;
using Puppy.Repositories.Interfaces;

namespace Puppy.Repositories;

public class LikeRepository : ILikeRepository
{
    private readonly AppDbContext _context;

    public LikeRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Like?> GetLikeByUserAndPost(int postId, int userId)
    {
        var like = await _context.Like.Where(x => x.PostId == postId && x.UserId == userId)
            .FirstOrDefaultAsync();
        return like;
    }
    public async Task<List<Like>?> GetLikesByPost(int postId)
    {
        var like = await _context.Like.Where(x => x.PostId == postId)
            .ToListAsync();
        return like;
    }
    public async Task<List<Like>?> GetLikesByUser(int userId)
    {
        var like = await _context.Like.Where(x => x.UserId == userId)
            .Include(x=>x.User)
            .Include(x=>x.Post)
            .Include(x=>x.Post.User)
            .ToListAsync();
        return like;
    }
    public async Task<Like?> LikePost(int postId, int userId)
    {
        var existingLike = await _context.Like.FirstOrDefaultAsync(l => l.UserId == userId && l.PostId == postId);
        if (existingLike != null)
        {
            return null;
        }

        var newLike = new Like()
        {
            UserId = userId,
            PostId = postId,
        };

        _context.Like.Add(newLike);
        await _context.SaveChangesAsync();
        return newLike;
    }

    public async Task<Like?> Unlike(int postId, int userId)
    {
        var like = await _context.Like.FirstOrDefaultAsync(l => l.PostId == postId && l.UserId == userId);
        if (like != null) _context.Like.Remove(like);
        await _context.SaveChangesAsync();
        return like;
    }
    public int GetTotal(int postId)
    {
        var count = _context.Like.Count(comment => comment.PostId == postId);
        return count;
    }
}