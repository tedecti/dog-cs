using Puppy.Models;

namespace Puppy.Repository.Interfaces;

public interface ILikeRepository
{
    Task<Like> LikePost(int postId, int userId);
    Task<Like> Unlike(int postId, int userId);
}