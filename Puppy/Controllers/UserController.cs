using AutoMapper;
using Curs.Data;
using Curs.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Puppy.Models.Dto;
using Puppy.Repository.IRepository;
using Puppy.Services.Interfaces;

namespace Puppy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IFileRepository _fileRepo;
        private readonly IUserService _userService;

        public UserController(AppDbContext context, IMapper mapper, IFileRepository fileRepo, IUserService userService)
        {
            _context = context;
            _mapper = mapper;
            _fileRepo = fileRepo;
            _userService = userService;
        }

       
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.GetUsers();

            if (users == null || !users.Any())
            {
                return NotFound();
            }

            var userResponses = _mapper.Map<IEnumerable<ShortUserDto>>(users);

            return Ok(userResponses);
        }

        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _userService.GetUser(id);
            
            if (user == null)
            {
                return NotFound();
            }

            var response = _mapper.Map<UserResponseDto>(user);
            return Ok(response);
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<ActionResult<UserResponseDto>> GetMe()
        {
            var userId = Convert.ToInt32(HttpContext.User.Identity.Name);
            
            var user = await _userService.GetUser(userId);
            
            if (user == null)
            {
                return NotFound();
            }
            
            return _mapper.Map<UserResponseDto>(user);
        }
        
        
        [HttpPut("edit")]
        [Authorize]
        public async Task<IActionResult> PutUser(UpdateUserDto user)
        {
            var id = Convert.ToInt32(User.Identity.Name);
            var existingUser = await _userService.GetUser(id);
            if (existingUser == null)
            {
                return NotFound();
            }
            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;

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

            var userId = Convert.ToInt32(HttpContext.User.Identity.Name);

            Console.WriteLine("a");
            string fName = await _fileRepo.SaveFile(file);

            var user = await _userService.GetUser(userId);
            user.Avatar = fName;

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}