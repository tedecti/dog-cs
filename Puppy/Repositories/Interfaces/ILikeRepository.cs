using Puppy.Models;

namespace Puppy.Repositories.Interfaces;

public interface ILikeRepository
{
    Task<Like?> GetLikeByUserAndPost(int postId, int userId);
    Task<List<Like>?> GetLikesByPost(int postId);
    Task<List<Like>?> GetLikesByUser(int userId);
    Task<Like?> LikePost(int postId, int userId);
    Task<Like?> Unlike(int postId, int userId);
    int GetTotal(int postId);
}