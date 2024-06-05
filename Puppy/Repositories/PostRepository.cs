using Microsoft.EntityFrameworkCore;
using Puppy.Data;
using Puppy.Models;
using Puppy.Models.Dto.PostDtos;
using Puppy.Repositories.Interfaces;

namespace Puppy.Repositories;

public class PostRepository : IPostRepository
{
    private readonly AppDbContext _context;
    private readonly IFileRepository _fileRepo;

    public PostRepository(AppDbContext context, IFileRepository fileRepo)
    {
        _context = context;
        _fileRepo = fileRepo;
    }

    public async Task<IEnumerable<Post>> GetAllPosts()
    {
        var allPosts = await _context.Post
            .Include(p => p.User)
            .OrderByDescending(p => p.UploadDate)
            .ToListAsync();
        return allPosts;
    }

    public async Task<IEnumerable<Post>> GetFriendPostsAsync(IEnumerable<int> friendIds)
    {
        return await _context.Post
            .Where(p => friendIds.Contains(p.UserId))
            .Include(p => p.User)
            .ToListAsync();
    }

    public async Task<Post?> GetPostById(int postId)
    {
        var post = await _context.Post.Include(p => p.User).FirstOrDefaultAsync(p => p.Id == postId);
        return post;
    }

    public async Task<Post> CreatePost(UploadPostRequestDto uploadPostRequestDto, int userId)
    {
        var imgs = new List<string>();
        foreach (var file in uploadPostRequestDto.Imgs)
        {
            imgs.Add(await _fileRepo.SaveFile(file));
        }

        var newPost = new Post()
        {
            Title = uploadPostRequestDto.Title,
            Description = uploadPostRequestDto.Description,
            UserId = userId,
            Imgs = imgs.ToArray(),
            UploadDate = DateTime.UtcNow
        };
        _context.Post.Add(newPost);
        await _context.SaveChangesAsync();
        return newPost;
    }

    public async Task<Post?> EditPost(UploadPostRequestDto editPostRequestDto, int postId)
    {
        var existingPost = await GetPostById(postId);
        if (existingPost == null) return null;
        
        var postImgs = existingPost.Imgs;
        foreach (var img in postImgs)
        {
            await _fileRepo.DeleteFileFromStorage(img);
        }
        
        var imgs = new List<string>();
        foreach (var file in editPostRequestDto.Imgs)
        {
            imgs.Add(await _fileRepo.SaveFile(file));
        }
        
        existingPost.Title = editPostRequestDto.Title;
        existingPost.Description = editPostRequestDto.Description;
        existingPost.Imgs = imgs.ToArray();
        
        await _context.SaveChangesAsync();
        return existingPost;
    }

    public async Task<Post> DeletePost(int postId)
    {
        var post = await GetPostById(postId);
        _context.Post.Remove(post);
        await _context.SaveChangesAsync();
        return post;
    }
}