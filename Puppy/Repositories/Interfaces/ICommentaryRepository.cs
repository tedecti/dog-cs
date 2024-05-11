using Puppy.Models;
using Puppy.Models.Dto.PostDtos.CommentaryDtos;

namespace Puppy.Repositories.Interfaces;

public interface ICommentaryRepository
{
    Task<Commentary> CreateCommentary(AddCommentaryRequestDto addCommentaryRequestDto, int userId, int postId);
    Task<Commentary> EditCommentary(AddCommentaryRequestDto addCommentaryRequestDto, int commentaryId);
    Task<Commentary> DeleteCommentary(int commentId);
    Task<IEnumerable<Commentary>?> GetCommentariesByPost(int postId);
    Task<List<Commentary>> GetCommentariesByUser(int userId);
    Task<Commentary> GetCommentById(int commentaryId);
    int GetTotal(int postId);
}