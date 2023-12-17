using Curs.Models;
using Microsoft.EntityFrameworkCore;
using Puppy.Data;
using Puppy.Services.Interfaces;

namespace Puppy.Services;

public class LikeService : ILikeService
{
    private readonly AppDbContext _context;

    public LikeService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Like> GetLike(int postId, int userId)
    {
        var like = await _context.Like.Where(x => x.PostId == postId && x.UserId == userId)
            .FirstOrDefaultAsync();
        return like;
    }
}