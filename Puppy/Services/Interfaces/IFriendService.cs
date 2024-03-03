using Puppy.Models;

namespace Puppy.Services.Interfaces;

public interface IFriendService
{
    Task<bool> IsFriend(int userId, int currentUserId);
    Task<IEnumerable<Friend>> GetFriends(int userId);
}