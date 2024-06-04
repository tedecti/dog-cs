using Microsoft.EntityFrameworkCore;
using Puppy.Data;
using Puppy.Models;
using Puppy.Repositories.Interfaces;

namespace Puppy.Repositories;

public class FollowerRepository : IFollowerRepository
{
    private readonly AppDbContext _context;

    public FollowerRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Friend?> Follow(int id, int userId)
    {
        var existingFriend = await _context.Friend.FirstOrDefaultAsync(
            f => f.FollowerId == id && f.UserId == userId);
        if (existingFriend != null)
        {
            return null;
        }

        var newFollow = new Friend
        {
            UserId = userId,
            FollowerId = id
        };
        _context.Friend.Add(newFollow);
        await _context.SaveChangesAsync();
        return newFollow;
    }

    public async Task<Friend?> Unfollow(int id, int userId)
    {
        var follower = await _context.Friend.FirstOrDefaultAsync(f => f.FollowerId == id && f.UserId == userId);
        if (follower != null) _context.Remove(follower);
        await _context.SaveChangesAsync();
        return follower;
    }

    public async Task<bool> IsFollowed(int userId, int currentUserId)
    {
        var follower = await _context.Friend.Where(x => x.FollowerId == userId && x.UserId == currentUserId)
            .FirstOrDefaultAsync();
        Console.WriteLine(follower);
        return follower != null;
    }

    public async Task<List<Friend>> GetFollowers(int userId)
    {
        var friends = await _context.Friend
            .Where(f => f.FollowerId == userId)
            .Include(f => f.User)
            .Include(f=>f.Follower).ToListAsync();
        return friends;
    }

    public async Task<List<Friend>> GetMyFollowers(int userId)
    {
        var myFollowers = await _context.Friend
            .Where(f => f.UserId == userId)
            .Include(f => f.User)
            .Include(f=>f.Follower).ToListAsync();
        return myFollowers;
    }

    public async Task<Friend?> GetFollower(int followerId)
    {
        var friend = await _context.Friend.Where(f => f.FollowerId == followerId).FirstOrDefaultAsync();
        return friend;
    }
}