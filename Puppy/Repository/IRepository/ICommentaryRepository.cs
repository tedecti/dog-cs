using Curs.Models;
using Puppy.Models.Dto;

namespace Puppy.Repository.IRepository;

public interface ICommentaryRepository
{
    Task<Commentary> CreateCommentary(AddCommentaryRequestDto addCommentaryRequestDto, int userId, int postId);
    Task<Commentary> EditCommentary(AddCommentaryRequestDto addCommentaryRequestDto, int commentaryId);
    Task<Commentary> DeleteCommentary(int commentId);
}