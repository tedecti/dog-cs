using Puppy.Models;

namespace Puppy.Services.Interfaces;

public interface IUserService
{
    Task<User> GetUser(int userId);
    Task<IEnumerable<User>> GetUsers();
    bool IsUniqueEmail(string email);
    bool IsUnique (string email, string username);
}