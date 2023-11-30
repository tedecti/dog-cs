using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
        private readonly IMapper _mapper;

        public CommentaryController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        // GET: api/Commentary/5
        [HttpGet("{postId}")]
        public async Task<ActionResult<PostCommentaries>> GetCommentary(int postId)
        {
            try
            {
                var comments = await _context.Commentary 
                    .Include(x=>x.User)
                    .Where(comment => comment.PostId == postId)
                    .ToListAsync();

                if (comments == null )
                {
                    return NotFound();
                }

                var commentDtos = _mapper.Map<IEnumerable<GetCommentsDto>>(comments);

                int count = _context.Commentary
                    .Include(x => x.User)
                    .Where(comment => comment.PostId == postId).Count();
                
                var response = new PostCommentaries()
                {
                    comments = commentDtos,
                    total = count,
                };
                
                return Ok(response);
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
        [Route("{postId}")]
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Commentary>> PostCommentary(int postId, AddCommentaryRequestDto commentary)
        {
          if (_context.Commentary == null)
          {
              return Problem("Entity set 'AppDbContext.Commentary'  is null.");
          }
          var userId = HttpContext.User.Identity.Name;
          var newCommentary = new Commentary()
          {
              PostId = postId,
              UserId = Convert.ToInt32(userId),
              Text = commentary.Text,
              UploadDate = DateTime.UtcNow
          };
            _context.Commentary.Add(newCommentary);
            await _context.SaveChangesAsync();

            return StatusCode(201);
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
