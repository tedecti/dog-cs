using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Puppy.Data;
using Puppy.Models;
using Puppy.Models.Dto;
using Puppy.Models.Dto.PostDtos;
using Puppy.Repository;
using Puppy.Repository.Interfaces;

namespace Puppy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostRepository _postRepository;
        private readonly IPostService _postService;
        private readonly IMapper _mapper;

        public PostController(IPostService postService, IPostRepository postRepository, IMapper mapper)
        {
            _postService = postService;
            _postRepository = postRepository;
            _mapper = mapper;
        }

        // GET: api/Post
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<GetPostDto>>> GetPosts()
        {
            var userId = Convert.ToInt32(HttpContext.User.Identity?.Name);

            var allPosts = await _postService.GetPosts(userId);

            var dtos = _mapper.Map<IEnumerable<GetPostDto>>(allPosts);
            return Ok(dtos);
        }


        // GET: api/Post/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GetPostDto>> GetPost(int id)
        {

            var post = await _postService.GetPostById(id);
            

            if (post == null)
            {
                return NotFound();
            }

            
            var dtos = _mapper.Map<GetPostDto>(post);
            
            return Ok(dtos);
        }
        
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutPost([FromBody] UploadPostRequestDto editPostRequestDto, int id)
        {
            var post = await _postService.GetPostById(id);
            var userId = Convert.ToInt32(HttpContext.User.Identity?.Name);
            if (userId != post.UserId)
            {
                return BadRequest();
            }
            await _postRepository.EditPost(editPostRequestDto, id);

            return NoContent();
        }

        
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Post>> PostPost([FromForm] UploadPostRequestDto post)
        {
            var userId = Convert.ToInt32(HttpContext.User.Identity?.Name);

            await _postRepository.CreatePost(post, userId); 
            
            return StatusCode(201);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeletePost(int id)
        {
            await _postRepository.DeletePost(id);
            return NoContent();
        }
        
    }
}