using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Curs.Data;
using Curs.Models;
using Microsoft.AspNetCore.Authorization;
using Puppy.Data;
using Puppy.Models.Dto;

namespace Puppy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LikeController : ControllerBase
    {
        private readonly AppDbContext _context;

        public LikeController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Like/1
        [HttpGet("{postId}")]
        [Authorize]
        public async Task<ActionResult<bool>> GetLike(int postId)
        {
          var userId = Convert.ToInt32(HttpContext.User.Identity?.Name);

          var like = await _context.Like.Where(x => x.PostId == postId && x.UserId == userId)
              .FirstOrDefaultAsync();
          if (like == null) return false;
          return true;
        }
        
        // POST: api/Like/1
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{PostId}")]
        [Authorize]
        public async Task<ActionResult<Like>> PostLike(int postId)
        {
            var userId = HttpContext.User.Identity?.Name;
            int userIdInt = Convert.ToInt32(userId);
            
            
            var existingLike = await _context.Like.FirstOrDefaultAsync(l => l.UserId.ToString() == userId && l.PostId == postId);
            if (existingLike != null)
            {
                return BadRequest("Like is already exist.");
            }

            if (postId == null)
            {
                return NotFound();
            }
            
            var newLike = new Like()
            {
                UserId = userIdInt,
                PostId = postId,
            };

            _context.Like.Add(newLike);
            await _context.SaveChangesAsync();

            return StatusCode(201);
        }

        // DELETE: api/Like/5
        [HttpDelete("{PostId}")]
        [Authorize]
        public async Task<IActionResult> DeleteLike(int PostId)
        {
            var userId = HttpContext.User.Identity?.Name;
            var like = await _context.Like.FirstOrDefaultAsync(l => l.PostId == PostId && l.UserId.ToString() == userId);
            if (like == null)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
