using Microsoft.EntityFrameworkCore;
using Puppy.Data;
using Puppy.Models;
using Puppy.Repositories.Interfaces;

namespace Puppy.Repositories;

public class SearchRepository : ISearchRepository
{
    private readonly AppDbContext _context;

    public SearchRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Post>> SearchPosts(string query)
    {
        var postResult = await _context.Post.Include(x => x.User).Where(p =>
                p.Title.ToLower().Contains(query.ToLower()))
            .ToListAsync();
        return postResult;
    }

    public async Task<IEnumerable<User>> SearchUsers(string query)
    {
        var userResult = await _context.Users.Where(u => u.Username.ToLower().Contains(query.ToLower()))
            .ToListAsync();
        return userResult;
    }
}