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
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Like>>> GetLike()
        {
          if (_context.Like == null)
          {
              return NotFound();
          }
            return await _context.Like.ToListAsync();
        }
        
        // POST: api/Like
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Like>> PostLike(AddLikeRequestDto like)
        {
          if (_context.Like == null)
          {
              return Problem("Entity set 'AppDbContext.Like'  is null.");
          }
          var existingLike = await _context.Like.FirstOrDefaultAsync(l => l.UserId == like.UserId && l.PostId == like.PostId);
          if (existingLike != null)
          {
              return BadRequest("Like is already exist.");
          }

          if (like.PostId == null)
          {
              return NotFound();
          }
          var newLike = new Like()
          {
              UserId = like.UserId,
              PostId = like.PostId,
             
          };
            _context.Like.Add(newLike);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLike", like);
        }

        // DELETE: api/Like/5
        [HttpDelete("{PostId}/Unlike")]
        [Authorize]
        public async Task<IActionResult> DeleteLike(int PostId)
        {
            var like = await _context.Like.FirstOrDefaultAsync(l => l.PostId == PostId);
            if (_context.Like == null)
            {
                return BadRequest(like);
            }
            if (like == null)
            {
                return BadRequest(like);
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
