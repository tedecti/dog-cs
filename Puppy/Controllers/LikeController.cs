using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Puppy.Data;
using Puppy.Models;
using Puppy.Repositories.Interfaces;
using Puppy.Services.Interfaces;

namespace Puppy.Controllers
{
    [Route("api")]
    [ApiController]
    public class LikeController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILikeRepository _likeRepository;

        public LikeController(AppDbContext context, ILikeRepository likeRepository)
        {
            _context = context;
            _likeRepository = likeRepository;
        }

        // GET: api/Like/1
        [HttpGet("like/{postId}")]
        [Authorize]
        public async Task<ActionResult<bool>> GetLike(int postId)
        {
            var userId = Convert.ToInt32(HttpContext.User.Identity?.Name);

            var like = await _likeRepository.GetLike(postId, userId);
            if (like == null) return false;
            return true;
        }

        // POST: api/Like/1
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("like/{PostId}")]
        [Authorize]
        public async Task<ActionResult<Like>> PostLike(int postId)
        {
            var userId = Convert.ToInt32(HttpContext.User.Identity?.Name);


            var existingLike = await _likeRepository.GetLike(postId, userId);
            if (existingLike != null)
            {
                return BadRequest("Like is already exist.");
            }

            if (postId == null)
            {
                return NotFound();
            }

            await _likeRepository.LikePost(postId, userId);

            return StatusCode(201);
        }

        // // DELETE: api/Like/5
        [HttpDelete("unlike/{PostId}")]
        [Authorize]
        public async Task<IActionResult> DeleteLike(int postId)
        {
            var userId = Convert.ToInt32(HttpContext.User.Identity?.Name);
            var like = await _likeRepository.GetLike(postId, userId);
            if (like == null)
            {
                return NotFound();
            }

            await _likeRepository.Unlike(postId, userId);
            return NoContent();
        }
    }
}