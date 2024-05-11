using Puppy.Models;

namespace Puppy.Repositories.Interfaces;

public interface ILikeRepository
{
    Task<Like?> GetLike(int postId, int userId);
    Task<Like?> LikePost(int postId, int userId);
    Task<Like?> Unlike(int postId, int userId);
}