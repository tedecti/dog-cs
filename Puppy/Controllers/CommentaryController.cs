using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Puppy.Data;
using Puppy.Models;
using Puppy.Models.Dto;
using Puppy.Models.Dto.PostDtos.CommentaryDtos;
using Puppy.Repository.Interfaces;
using Puppy.Services.Interfaces;

namespace Puppy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentaryController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICommentaryService _commentaryService;
        private readonly ICommentaryRepository _commentaryRepository;

        public CommentaryController(IMapper mapper, ICommentaryService commentaryService, ICommentaryRepository commentaryRepository)
        {
            _mapper = mapper;
            _commentaryService = commentaryService;
            _commentaryRepository = commentaryRepository;
        }

        // GET: api/Commentary/5
        [HttpGet("{postId}")]
        public async Task<ActionResult<PostCommentariesDto>> GetCommentary(int postId)
        {
            var comments = await _commentaryService.GetCommentaries(postId);
            if (string.IsNullOrEmpty(comments.ToString()))
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
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutCommentary(int id, AddCommentaryRequestDto editCommentaryRequestDto)
        {
            // var userId = Convert.ToInt32(HttpContext.User.Identity?.Name);
            await _commentaryRepository.EditCommentary(editCommentaryRequestDto, id);
            return NoContent();
        }


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
        public async Task<IActionResult> DeleteCommentary(int commentId)
        {
            await _commentaryRepository.DeleteCommentary(commentId);
            return NoContent();
        }
    }
}