using Microsoft.AspNetCore.Mvc;
using Puppy.Models.Dto.AuthDtos;
using Puppy.Repositories.Interfaces;
using Puppy.Services.Interfaces;

namespace Puppy.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepo;

        public AuthController(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto model)
        {
            var response = await _userRepo.Login(model);

            if (response?.User == null || string.IsNullOrEmpty(response.Token))
            {
                return BadRequest(new { message = "Email or password incorrect" });
            }

            return StatusCode(200, response);
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDto model)
        {
            var isUnique = _userRepo.IsUnique(model.Email, model.Username);
            if (!isUnique)
            {
                return BadRequest("Username or email is not unique");
            }

            var user = await _userRepo.Register(model);

            if (user == null)
            {
                return BadRequest(new { message = "Error while register" });
            }

            return StatusCode(201);
        }

        [HttpPost("unique")]
        public bool CheckUnique([FromBody] CheckUniqueDto uniqueDto)
        {
            var isUnique = _userRepo.IsUniqueEmail(uniqueDto.Email);
            return isUnique;
        }
    }
}