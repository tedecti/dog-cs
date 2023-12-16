using Curs.Models;

namespace Puppy.Repository.IRepository;

public interface ILikeRepository
{
    Task<Like> LikePost(int postId, int userId);
    Task<Like> Unlike(int postId, int userId);
}