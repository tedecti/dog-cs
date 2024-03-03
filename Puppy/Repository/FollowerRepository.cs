using Microsoft.EntityFrameworkCore;
using Puppy.Data;
using Puppy.Models;

namespace Puppy.Repository;

public class FollowerRepository
{
    private readonly AppDbContext _context;

    public FollowerRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Friend> Follow(int id, int userId)
    {
        var existingFriend = await _context.Friend.FirstOrDefaultAsync(
            f => f.FollowerId == userId && f.UserId == id);
        if (existingFriend != null)
        {
            return null;
        }

        var newFollow = new Friend()
        {
            UserId = userId,
            FollowerId = id
        };
        _context.Friend.Add(newFollow);
        await _context.SaveChangesAsync();
        return newFollow;
    }

    public async Task<Friend> Unfollow(int id, int userId)
    {
        var follower = await _context.Friend.FirstOrDefaultAsync(f => f.FollowerId == id && f.UserId == userId);
        if (follower != null) _context.Remove(follower);
        await _context.SaveChangesAsync();
        return follower;
    }
}