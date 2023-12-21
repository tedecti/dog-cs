using Curs.Models;
using Puppy.Data;
using Puppy.Models.Dto;
using Puppy.Repository.IRepository;
using Puppy.Services.Interfaces;

namespace Puppy.Repository;

public class CommentaryRepository : ICommentaryRepository
{
    private readonly AppDbContext _context;
    private readonly ICommentaryService _commentaryService;

    public CommentaryRepository(AppDbContext context, ICommentaryService commentaryService)
    {
        _context = context;
        _commentaryService = commentaryService;
    }
    public async Task<Commentary> CreateCommentary(AddCommentaryRequestDto addCommentaryRequestDto, int userId, int postId)
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

    public async Task<Commentary> EditCommentary(AddCommentaryRequestDto editCommentaryRequestDto, int userId, int commentaryId)
    {
        var existingCommentary = await _commentaryService.GetComment(commentaryId);
        existingCommentary.Text = editCommentaryRequestDto.Text;
        await _context.SaveChangesAsync();
        return existingCommentary;
    }
}