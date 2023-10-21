using System;
using System.Collections.Generic;
using System.Linq;
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
    public class CommentaryController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CommentaryController(AppDbContext context)
        {
            _context = context;
        }
        
        // GET: api/Commentary/5
        [HttpGet]
        [Route("{PostId}")]
        public async Task<ActionResult<Commentary>> GetCommentary(int PostId)
        {
            try
            {
                var comments = await _context.Commentary 
                    .Where(comment => comment.PostId == PostId)
                    .ToListAsync();

                if (comments == null || comments.Count == 0)
                {
                    return NotFound();
                }

                var commentDtos = comments.Select(comment => new GetCommentsDto
                {
                    PostId = comment.PostId,
                    UserId = comment.UserId,
                    Text = comment.Text
                });

                return Ok(commentDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }

        // PUT: api/Commentary/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutCommentary(int id, Commentary commentary)
        {
            if (id != commentary.Id)
            {
                return BadRequest();
            }

            _context.Entry(commentary).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommentaryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Commentary/1
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{PostId}")]
        [Authorize]
        public async Task<ActionResult<Commentary>> PostCommentary(AddCommentaryRequestDto commentary, int PostId)
        {
          if (_context.Commentary == null)
          {
              return Problem("Entity set 'AppDbContext.Commentary'  is null.");
          }
          var userId = HttpContext.User.Identity.Name;
          var newCommentary = new Commentary()
          {
              PostId = PostId,
              UserId = Convert.ToInt32(userId),
              Text = commentary.Text
          };
            _context.Commentary.Add(newCommentary);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCommentary", commentary);
        }

        // DELETE: api/Commentary/5
        [HttpDelete("{PostId}")]
        [Authorize]
        public async Task<IActionResult> DeleteCommentary(int PostId)
        {
            var UserId = HttpContext.User.Identity.Name;
            var commentary = await _context.Commentary.FirstOrDefaultAsync(c => c.PostId == PostId && c.UserId.ToString() == UserId);
            if (_context.Commentary == null)
            {
                return NotFound();
            }

            if (commentary == null)
            {
                return NotFound();
            }

            _context.Commentary.Remove(commentary);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CommentaryExists(int id)
        {
            return (_context.Commentary?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
