using Curs.Models;
using Puppy.Models.Dto;

namespace Puppy.Repository.IRepository;

public interface IPostRepository
{
    Task<Post> CreatePost(UploadPostRequestDto uploadPostRequestDto, int userId);
    Task<Post> EditPost(EditPostRequestDto editPostRequestDto, int postId);
    Task<Post> DeletePost(int postId);
}