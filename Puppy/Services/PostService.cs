using Microsoft.EntityFrameworkCore;
using Puppy.Data;
using Puppy.Models;
using Puppy.Models.Dto;
using Puppy.Repositories.Interfaces;
using Puppy.Services.Interfaces;

namespace Puppy.Services;

public class PostService : IPostService
{
    private readonly IPostRepository _postRepository;
    private readonly IFollowerRepository _followerRepository;

    public PostService(AppDbContext context, IPostRepository postRepository, IFollowerRepository followerRepository)
    {
        _postRepository = postRepository;
        _followerRepository = followerRepository;
    }

    public async Task<IEnumerable<Post>> GetFilteredPostsAsync(int userId)
    {
        var friends = await _followerRepository.GetFollowers(userId);

        if (!friends.Any()) return await _postRepository.GetAllPosts();
        var friendIds = friends.Select(f => f.FollowerId).ToList();
        var friendPosts = await _postRepository.GetFriendPostsAsync(friendIds);
        var allPosts = await _postRepository.GetAllPosts();
        var otherPosts = allPosts.Where(p => !friendIds.Contains(p.UserId));
        return friendPosts.Concat(otherPosts);
    }
}