using Puppy.Models;
using Puppy.Models.Dto;
using Puppy.Models.Dto.PostDtos.CommentaryDtos;

namespace Puppy.Repository.Interfaces;

public interface ICommentaryRepository
{
    Task<Commentary> CreateCommentary(AddCommentaryRequestDto addCommentaryRequestDto, int userId, int postId);
    Task<Commentary> EditCommentary(AddCommentaryRequestDto addCommentaryRequestDto, int commentaryId);
    Task<Commentary> DeleteCommentary(int commentId);
}