using Microsoft.EntityFrameworkCore;
using Puppy.Data;
using Puppy.Models;
using Puppy.Models.Dto.PostDtos.CommentaryDtos;
using Puppy.Repositories.Interfaces;

namespace Puppy.Repositories;

public class CommentaryRepository : ICommentaryRepository
{
    private readonly AppDbContext _context;

    public CommentaryRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Commentary> CreateCommentary(AddCommentaryRequestDto addCommentaryRequestDto, int userId,
        int postId)
    {
        var newCommentary = new Commentary()
        {
            PostId = postId,
            UserId = Convert.ToInt32(userId),
            Text = addCommentaryRequestDto.Text,
            UploadDate = DateTime.UtcNow
        };
        _context.Commentary.Add(newCommentary);
        await _context.SaveChangesAsync();
        return newCommentary;
    }

    public async Task<Commentary> EditCommentary(AddCommentaryRequestDto editCommentaryRequestDto, int commentaryId)
    {
        var existingCommentary = await GetCommentById(commentaryId);
        existingCommentary.Text = editCommentaryRequestDto.Text;
        await _context.SaveChangesAsync();
        return existingCommentary;
    }

    public async Task<Commentary> DeleteCommentary(int commentId)
    {
        var commentary = await GetCommentById(commentId);
        _context.Commentary.Remove(commentary);
        await _context.SaveChangesAsync();
        return commentary;
    }

    public async Task<IEnumerable<Commentary>?> GetCommentariesByPost(int postId)
    {
        var comments = await _context.Commentary
            .Include(x => x.User)
            .Where(comment => comment.PostId == postId)
            .ToListAsync();
        return comments;
    }

    public int GetTotal(int postId)
    {
        var count = _context.Commentary
            .Include(x => x.User).Count(comment => comment.PostId == postId);
        return count;
    }

    public async Task<Commentary> GetCommentById(int commentaryId)
    {
        var comment = await _context.Commentary.FirstAsync(c => c.Id == commentaryId);
        return comment;
    }

    public async Task<List<Commentary>> GetCommentariesByUser(int userId)
    {
        var comments = await _context.Commentary.Where(c => c.UserId == userId).ToListAsync();
        return comments;
    }
}