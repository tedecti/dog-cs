using Puppy.Models;

namespace Puppy.Services.Interfaces;

public interface ISearchService
{
    Task<IEnumerable<Post>> SearchPosts(string query);
    Task<IEnumerable<User>> SearchUsers(string query);
}