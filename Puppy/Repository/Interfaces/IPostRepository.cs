using Puppy.Models;
using Puppy.Models.Dto.PostDtos;

namespace Puppy.Repository.Interfaces;

public interface IPostRepository
{
    Task<Post> CreatePost(UploadPostRequestDto uploadPostRequestDto, int userId);
    Task<Post> EditPost(EditPostRequestDto editPostRequestDto, int postId);
    Task<Post> DeletePost(int postId);
}