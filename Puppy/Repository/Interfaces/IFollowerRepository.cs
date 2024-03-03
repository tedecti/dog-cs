using Puppy.Models;

namespace Puppy.Repository.Interfaces;

public interface IFollowerRepository
{
    Task<Friend> Follow(int id, int userId);
    Task<Friend> Unfollow(int id, int userId);
}