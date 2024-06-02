using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Puppy.Models.Dto.UserDtos;
using Puppy.Repositories.Interfaces;

namespace Puppy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IFileRepository _fileRepo;
        private readonly IUserRepository _userRepo;

        public UserController(IMapper mapper, IFileRepository fileRepo, IUserRepository userRepo)
        {
            _mapper = mapper;
            _fileRepo = fileRepo;
            _userRepo = userRepo;
        }


        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userRepo.GetUsers();

            if (!users.Any())
            {
                return NotFound();
            }

            var userResponses = _mapper.Map<IEnumerable<ShortUserDto>>(users);

            return Ok(userResponses);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _userRepo.GetUser(id);

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
            var userId = Convert.ToInt32(HttpContext.User.Identity?.Name);

            var user = await _userRepo.GetUser(userId);

            if (user == null)
            {
                return NotFound();
            }
            
           
            return _mapper.Map<UserResponseDto>(user);
        }


        [HttpPut("{id}")]

        [Authorize]
        public async Task<IActionResult> PutUser(int id, [FromBody]UpdateUserDto updateUserDto)
        {
            var existingUser = await _userRepo.Edit(updateUserDto, id);
            if (existingUser == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        [Authorize]
        [HttpPost("upload")]
        public async Task<IActionResult> UploadAvatar()
        {
            var httpRequest = HttpContext.Request;
            var file = httpRequest.Form.Files["image"];
            var userId = Convert.ToInt32(HttpContext.User.Identity?.Name);
            string fName = await _fileRepo.SaveFile(file);
            await _userRepo.UploadAvatar(userId, fName);
            return Ok();
        }
    }
}