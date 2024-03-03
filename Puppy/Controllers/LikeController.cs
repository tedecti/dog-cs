using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Puppy.Data;
using Puppy.Models;
using Puppy.Models.Dto;
using Puppy.Repository.Interfaces;
using Puppy.Services.Interfaces;

namespace Puppy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LikeController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILikeRepository _likeRepository;
        private readonly ILikeService _likeService;

        public LikeController(AppDbContext context, ILikeRepository likeRepository, ILikeService likeService)
        {
            _context = context;
            _likeRepository = likeRepository;
            _likeService = likeService;
        }

        // GET: api/Like/1
        [HttpGet("{postId}")]
        [Authorize]
        public async Task<ActionResult<bool>> GetLike(int postId)
        {
          var userId = Convert.ToInt32(HttpContext.User.Identity?.Name);

          var like = await _likeService.GetLike(postId, userId); 
          if (like == null) return false;
          return true;
        }
        
        // POST: api/Like/1
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{PostId}")]
        [Authorize]
        public async Task<ActionResult<Like>> PostLike(int postId)
        {
            var userId = Convert.ToInt32(HttpContext.User.Identity?.Name);


            var existingLike = await _likeService.GetLike(postId, userId);
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

        // DELETE: api/Like/5
        [HttpDelete("{PostId}")]
        [Authorize]
        public async Task<IActionResult> DeleteLike(int postId)
        {
            var userId = Convert.ToInt32(HttpContext.User.Identity?.Name);
            var like = await _likeService.GetLike(postId, userId);
            if (like == null)
            {
                return NotFound();
            }

            await _likeRepository.Unlike(postId, userId);
            return NoContent();
        }
    }
}
