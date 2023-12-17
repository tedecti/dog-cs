using Curs.Models;

namespace Puppy.Services.Interfaces;

public interface ILikeService
{
    Task<Like> GetLike(int postId, int userId);
}