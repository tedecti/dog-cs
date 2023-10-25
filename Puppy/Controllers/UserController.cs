using AutoMapper;
using Curs.Data;
using Curs.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Puppy.Models.Dto;
using Puppy.Repository.IRepository;

namespace Puppy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _environment;
        private readonly IFileRepository _fileRepo;

        public UserController(AppDbContext context, IMapper mapper, IWebHostEnvironment environment, IFileRepository fileRepo)
        {
            _context = context;
            _mapper = mapper;
            _environment = environment;
            _fileRepo = fileRepo;
        }

        // GET: api/User
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShortUserDto>>> GetUsers()
        {
            if (_context.Users == null)
            {
                return NotFound();
            }

            var users = await _context.Users.ToListAsync();

            if (users == null || !users.Any())
            {
                return NoContent();
            }

            var userResponses = _mapper.Map<IEnumerable<ShortUserDto>>(users);

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

            var user = await _context.Users.Include(x => x.Pets).Include(x=>x.Posts).FirstAsync(x => x.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            return _mapper.Map<UserResponseDto>(user);
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<ActionResult<UserResponseDto>> GetMe()
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            var userId = HttpContext.User.Identity.Name;
            var user = await _context.Users.Include(x => x.Pets).Include(x=>x.Posts).Include(x=>x.Friends).Include(x=>x.Followers).FirstAsync(x => x.Id == Convert.ToInt32(userId));
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
        [HttpPost("upload")]
        public async Task<IActionResult> UploadAvatar()
        {
            var httpRequest = HttpContext.Request;

            var file = httpRequest.Form.Files["image"];

            var userId = HttpContext.User.Identity.Name;

            string fName = await _fileRepo.SaveFile(file);

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id.ToString() == userId);
            user.Avatar = fName;

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}