using Puppy.Models;

namespace Puppy.Repository.Interfaces;

public interface IPostService
{
    Task<IEnumerable<Post>> GetPosts(int userId);
    Task<Post> GetPostById(int postId);
}