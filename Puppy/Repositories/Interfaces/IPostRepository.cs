using Puppy.Models;
using Puppy.Models.Dto.PostDtos;

namespace Puppy.Repositories.Interfaces;

public interface IPostRepository
{
    Task<Post> CreatePost(UploadPostRequestDto uploadPostRequestDto, int userId);
    Task<Post?> EditPost(UploadPostRequestDto editPostRequestDto, int postId);
    Task<Post> DeletePost(int postId);
    Task<IEnumerable<Post>> GetFriendPostsAsync(IEnumerable<int> friendIds);
    Task<Post?> GetPostById(int postId);
    Task<IEnumerable<Post>> GetAllPosts();
}