using Puppy.Models;

namespace Puppy.Repositories.Interfaces;

public interface IFollowerRepository
{
    Task<Friend?> Follow(int id, int userId);
    Task<Friend?> Unfollow(int id, int userId);
    Task<bool> IsFollowed(int userId, int currentUserId);
    Task<List<Friend>> GetFollowers(int id);
    Task<List<Friend>> GetMyFollowers(int userId);
}