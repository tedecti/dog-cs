using Curs.Models;

namespace Puppy.Repository;

public interface IPostService
{
    Task<IEnumerable<Post>> GetPosts(int userId);
    Task<Post> GetPostById(int postId);
}