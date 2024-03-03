using Microsoft.EntityFrameworkCore;
using Puppy.Data;
using Puppy.Models;
using Puppy.Repository.Interfaces;

namespace Puppy.Repository;

public class LikeRepository : ILikeRepository
{
    private readonly AppDbContext _context;

    public LikeRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Like> LikePost(int postId, int userId)
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

    public async Task<Like> Unlike(int postId, int userId)
    {
        var like = await _context.Like.FirstOrDefaultAsync(l => l.PostId == postId && l.UserId == userId);
        if (like != null) _context.Like.Remove(like);
        await _context.SaveChangesAsync();
        return like;
    }
}