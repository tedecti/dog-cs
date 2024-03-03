using Puppy.Models;

namespace Puppy.Services.Interfaces;

public interface IFollowerService
{
    Task<bool> IsFollowed(int userId, int currentUserId);
    Task<IEnumerable<Friend>> GetFollowers(int userId);
}