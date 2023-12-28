using Curs.Models;
using Microsoft.EntityFrameworkCore;
using Puppy.Data;
using Puppy.Services.Interfaces;

namespace Puppy.Services;

public class FriendService : IFriendService
{
    private readonly AppDbContext _context;

    public FriendService(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<bool> IsFriend(int userId, int currentUserId)
    {
        var follower = await _context.Friend.Where(x => x.FollowerId == currentUserId && x.UserId == userId)
            .FirstOrDefaultAsync();
        return follower != null;
    }

    public async Task<IEnumerable<Friend>> GetFriends(int userId)
    {
        var friend = await _context.Friend.Where(f=> f.FollowerId == userId).Include(f=>f.User).ToListAsync();
        return friend;
    }
}