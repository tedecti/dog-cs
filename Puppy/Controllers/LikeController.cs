using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Puppy.Models;
using Puppy.Models.Dto.PostDtos.LikeDtos;
using Puppy.Repositories.Interfaces;

namespace Puppy.Controllers
{
    [Route("api/like")]
    [ApiController]
    public class LikeController(IMapper mapper, ILikeRepository likeRepository) : ControllerBase
    {
        [HttpGet("{postId}")]
        [Authorize]
        public async Task<ActionResult<bool>> GetLike(int postId)
        {
            var userId = Convert.ToInt32(HttpContext.User.Identity?.Name);
            var like = await likeRepository.GetLikeByUserAndPost(postId, userId);
            return like != null;
        }

        [HttpGet("{postId}/post")]
        public async Task<IActionResult> GetLikeByPost(int postId)
        {
            var total = likeRepository.GetTotal(postId);
            var response = new LikeDto()
            {
                PostId = postId,
                Total = total
            };
            return Ok(response);
        }

        [HttpGet("{userId}/user")]
        public async Task<IActionResult> GetLikeByUser(int userId)
        {
            var likes = await likeRepository.GetLikesByUser(userId);
            var response = mapper.Map<List<LikedPostsDto>>(likes);
            return Ok(response);
        }

        // POST: api/Like/1
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{postId}")]
        [Authorize]
        public async Task<ActionResult<Like>> PostLike(int postId)
        {
            var userId = Convert.ToInt32(HttpContext.User.Identity?.Name);


            var existingLike = await likeRepository.GetLikeByUserAndPost(postId, userId);
            if (existingLike != null)
            {
                return BadRequest("Like is already exist.");
            }

            if (postId == null)
            {
                return NotFound();
            }

            await likeRepository.LikePost(postId, userId);

            return StatusCode(201);
        }

        // // DELETE: api/Like/5
        [HttpDelete]
        [Route("{postId}")]
        [Authorize]
        public async Task<IActionResult> DeleteLike(int postId)
        {
            var userId = Convert.ToInt32(HttpContext.User.Identity?.Name);
            var like = await likeRepository.GetLikeByUserAndPost(postId, userId);
            if (like == null)
            {
                return NotFound();
            }

            await likeRepository.Unlike(postId, userId);
            return NoContent();
        }
    }
}