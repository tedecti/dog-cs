using Puppy.Models;

namespace Puppy.Services.Interfaces;

public interface IPostService
{
    Task<IEnumerable<Post>> GetFilteredPostsAsync(int userId);
}