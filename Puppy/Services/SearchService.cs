using Curs.Models;
using Microsoft.EntityFrameworkCore;
using Puppy.Data;
using Puppy.Services.Interfaces;

namespace Puppy.Services;

public class SearchService : ISearchService
{
    private readonly AppDbContext _context;

    public SearchService(AppDbContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<Post>> SearchPosts(string query)
    {
        var postResult = await _context.Post.Include(x=>x.User).Where(p =>
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