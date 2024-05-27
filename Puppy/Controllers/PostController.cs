using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Puppy.Data;
using Puppy.Models;
using Puppy.Models.Dto;
using Puppy.Models.Dto.PostDtos;
using Puppy.Repositories.Interfaces;
using Puppy.Services.Interfaces;

namespace Puppy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController(IPostService postService, IPostRepository postRepository, IMapper mapper)
        : ControllerBase
    {
        // GET: api/Post
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<GetPostDto>>> GetPosts()
        {
            var userId = Convert.ToInt32(HttpContext.User.Identity?.Name);

            var allPosts = await postService.GetFilteredPostsAsync(userId);

            var dtos = mapper.Map<IEnumerable<GetPostDto>>(allPosts);
            return Ok(dtos);
        }


        // GET: api/Post/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GetPostDto>> GetPost(int id)
        {
            var post = await postRepository.GetPostById(id);

            if (post == null)
            {
                return NotFound();
            }

            var dtos = mapper.Map<GetPostDto>(post);

            return Ok(dtos);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutPost([FromForm] UploadPostRequestDto editPostRequestDto, int id)
        {
            var post = await postRepository.GetPostById(id);
            var userId = Convert.ToInt32(HttpContext.User.Identity?.Name);
            if (post != null && userId != post.UserId)
            {
                return Unauthorized();
            }

            await postRepository.EditPost(editPostRequestDto, id);

            return NoContent();
        }


        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Post>> PostPost([FromForm] UploadPostRequestDto post)
        {
            var userId = Convert.ToInt32(HttpContext.User.Identity?.Name);

            await postRepository.CreatePost(post, userId);

            return StatusCode(201);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeletePost(int id)
        {
            await postRepository.DeletePost(id);
            return NoContent();
        }
    }
}