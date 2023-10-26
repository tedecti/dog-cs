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

        // GET: api/Like
        [HttpGet("{postId}")]
        [Authorize]
        public async Task<ActionResult<bool>> GetLike(string postId)
        {
          if (_context.Like == null)
          {
              return NotFound();
          }
          
          var userId = HttpContext.User.Identity.Name;

          if (string.IsNullOrEmpty(userId))
          {
              return Unauthorized();
          }

          var like = await _context.Like.Where(x => x.PostId.ToString() == postId && x.UserId.ToString() == userId)
              .FirstOrDefaultAsync();
          if (like == null) return false;
          return true;
        }
        
        // POST: api/Like/1
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{PostId}")]
        [Authorize]
        public async Task<ActionResult<Like>> PostLike(int PostId)
        {
            var userId = HttpContext.User.Identity.Name;
            int userIdInt = Convert.ToInt32(userId);
            if (userIdInt == null)
            {
                return Unauthorized();
            }

            if (_context.Like == null)
            {
                return Problem("Entity set 'AppDbContext.Like' is null.");
            }
            
            var existingLike = await _context.Like.FirstOrDefaultAsync(l => l.UserId.ToString() == userId && l.PostId == PostId);
            if (existingLike != null)
            {
                return BadRequest("Like is already exist.");
            }

            if (PostId == null)
            {
                return NotFound();
            }
            
            var newLike = new Like()
            {
                UserId = userIdInt,
                PostId = PostId,
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
            var UserId = HttpContext.User.Identity.Name;
            var like = await _context.Like.FirstOrDefaultAsync(l => l.PostId == PostId && l.UserId.ToString() == UserId);
            if (_context.Like == null)
            {
                return NotFound();
            }
            if (like == null)
            {
                return NotFound();
            }

            _context.Like.Remove(like);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LikeExists(int id)
        {
            return (_context.Like?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
