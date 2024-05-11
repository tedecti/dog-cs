using Puppy.Models;

namespace Puppy.Repositories.Interfaces;

public interface ISearchRepository
{
    Task<IEnumerable<Post>> SearchPosts(string query);
    Task<IEnumerable<User>> SearchUsers(string query);
}