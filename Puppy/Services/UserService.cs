using Microsoft.EntityFrameworkCore;
using Puppy.Data;
using Puppy.Models;
using Puppy.Services.Interfaces;

namespace Puppy.Services;

public class UserService : IUserService
{
    private readonly AppDbContext _context;

    public UserService(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<User> GetUser(int userId)
    {
        var user = await _context.Users.Include(x => x.Pets)
            .Include(x=>x.Posts)
            .Include(x=>x.Followers).ThenInclude(x=>x.Follower)
            .Include(x=>x.Friends).ThenInclude(x=>x.User)
            .FirstAsync(x => x.Id == Convert.ToInt32(userId));
        return user;
    }

    public async Task<IEnumerable<User>> GetUsers()
    {
        var users = await _context.Users.ToListAsync();
        return users;
    }
    public bool IsUniqueEmail(string email)
    {
        var userEmail = _context.Users.FirstOrDefault(x => x.Email == email);
        return userEmail == null;
    }

    public bool IsUnique(string email, string username)
    {
        var user = _context.Users.FirstOrDefault(x => x.Email == email || x.Username == username);
            
        return user == null;
    }
}