using System;
using System.Collections;
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
using Puppy.Data;
using Puppy.Models.Dto;
using Puppy.Models.Dto.PostDtos.CommentaryDtos;
using Puppy.Repository.IRepository;
using Puppy.Services.Interfaces;

namespace Puppy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentaryController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICommentaryService _commentaryService;
        private readonly ICommentaryRepository _commentaryRepository;

        public CommentaryController(AppDbContext context, IMapper mapper, ICommentaryService commentaryService, ICommentaryRepository commentaryRepository)
        {
            _context = context;
            _mapper = mapper;
            _commentaryService = commentaryService;
            _commentaryRepository = commentaryRepository;
        }

        // GET: api/Commentary/5
        [HttpGet("{postId}")]
        public async Task<ActionResult<PostCommentariesDto>> GetCommentary(int postId)
        {
            var comments = _commentaryService.GetCommentaries(postId);
            if (comments == null)
            {
                return NotFound();
            }

            var commentDtos = _mapper.Map<IEnumerable<GetCommentsDto>>(comments);

            var count = Convert.ToInt32(_commentaryService.GetTotal(postId));

            var response = new PostCommentariesDto()
            {
                Comments = commentDtos,
                Total = count,
            };

            return Ok(response);
        }

        // PUT: api/Commentary/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutCommentary(int id, AddCommentaryRequestDto editCommentaryRequestDto)
        {
            var userId = Convert.ToInt32(HttpContext.User.Identity?.Name);
            await _commentaryRepository.EditCommentary(editCommentaryRequestDto, userId, id);
            return NoContent();
        }

        // POST: api/Commentary/1
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Route("{postId}")]
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Commentary>> PostCommentary(int postId, AddCommentaryRequestDto commentary)
        {
            var userId = Convert.ToInt32(HttpContext.User.Identity?.Name);
            await _commentaryRepository.CreateCommentary(commentary, userId, postId);
            return StatusCode(201);
        }

        // DELETE: api/Commentary/5
        [HttpDelete("{PostId}")]
        [Authorize]
        public async Task<IActionResult> DeleteCommentary(int PostId)
        {
            var UserId = HttpContext.User.Identity?.Name;
            var commentary =
                await _context.Commentary.FirstOrDefaultAsync(c => c.PostId == PostId && c.UserId.ToString() == UserId);
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