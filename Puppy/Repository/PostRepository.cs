using Curs.Models;
using Puppy.Data;
using Puppy.Models.Dto;
using Puppy.Repository.IRepository;

namespace Puppy.Repository;

public class PostRepository : IPostRepository
{
    private readonly AppDbContext _context;
    private readonly IFileRepository _fileRepo;
    private readonly IPostService _postService;

    public PostRepository(AppDbContext context, IFileRepository fileRepo, IPostService postService)
    {
        _context = context;
        _fileRepo = fileRepo;
        _postService = postService;
    }

    public async Task<Post> CreatePost(UploadPostRequestDto uploadPostRequestDto, int userId)
    {
        List<string> imgs = new List<string>();
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

    public async Task<Post> EditPost(EditPostRequestDto editPostRequestDto, int postId)
    {
        var existingPost = await _postService.GetPostById(postId);
        existingPost.Title = editPostRequestDto.Title;
        existingPost.Description = editPostRequestDto.Description;
        await _context.SaveChangesAsync();
        return existingPost;
    }

    public async Task<Post> DeletePost(int postId)
    {
        var post = await _postService.GetPostById(postId);
        _context.Post.Remove(post);
        await _context.SaveChangesAsync();
        return post;
    }
}