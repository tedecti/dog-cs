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
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _environment;

        public UserController(AppDbContext context, IMapper mapper, IWebHostEnvironment environment)
        {
            _context = context;
            _mapper = mapper;
            _environment = environment;
        }

        // GET: api/User
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserResponseDto>>> GetUsers()
        {
            if (_context.Users == null)
            {
                return NotFound();
            }

            var users = await _context.Users.Include(x => x.Followers).ToListAsync();

            if (users == null || !users.Any())
            {
                return NoContent();
            }
            var userResponses = _mapper.Map<IEnumerable<UserResponseDto>>(users);

            return Ok(userResponses);

        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponseDto>> GetUser(int id)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users.Include(x => x.Pets).FirstAsync(x => x.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            return _mapper.Map<UserResponseDto>(user);
        }

        // PUT: api/User/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        private bool UserExists(int id)
        {
            return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        [Authorize]
        [HttpPost]
        [Route("{id}/upload")]
        public async Task<IActionResult> UploadAvatar(int id )
        {
            var httpRequest = HttpContext.Request;
            
            var file = httpRequest.Form.Files["image"];
            
            string fName = file.FileName;
            string path = Path.Combine(_environment.ContentRootPath, "Images", file.FileName);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            
            return Ok();
        }
        
    }
}