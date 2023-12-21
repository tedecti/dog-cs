using Curs.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Puppy.Data;
using Puppy.Services.Interfaces;

namespace Puppy.Services;

public class CommentaryService : ICommentaryService
{
    private readonly AppDbContext _context;

    public CommentaryService(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<Commentary>> GetCommentaries(int postId)
    {
        var comments = await _context.Commentary
            .Include(x => x.User)
            .Where(comment => comment.PostId == postId)
            .ToListAsync();
        return comments.IsNullOrEmpty() ? null : comments;
    }

    public Task<int> GetTotal(int postId)
    {
        var count = _context.Commentary
            .Include(x => x.User).Count(comment => comment.PostId == postId);
        return Task.FromResult(count);
    }

    public async Task<Commentary> GetComment(int commentaryId)
    {
        var comment = await _context.Commentary.FirstAsync(c => c.Id == commentaryId);
        return comment;
    }
}