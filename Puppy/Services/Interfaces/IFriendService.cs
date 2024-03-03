using Puppy.Models;

namespace Puppy.Services.Interfaces;

public interface IFriendService
{
    Task<bool> IsFollowed(int userId, int currentUserId);
    Task<IEnumerable<Friend>> GetFollowers(int userId);
}