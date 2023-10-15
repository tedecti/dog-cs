using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Puppy.Models.Dto;
using Puppy.Repository.IRepository;

namespace Puppy.Controllers
{
	[Route("api/UserAuth")]
	[ApiController]


	public class AuthController : ControllerBase
	{
		private readonly IUserRepository _userRepo;

		public AuthController(IUserRepository userRepo) { _userRepo = userRepo; }


		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginRequestDto model)
		{
			var response = await _userRepo.Login(model);

			if (response.User == null || string.IsNullOrEmpty(response.Token))
			{
				return BadRequest(new { message = "Email or password incorrect" });
			}
			return StatusCode(200, response);
		}


		[HttpPost("register")]
		public async Task<IActionResult> Register([FromBody] RegistrationRequestDto model)
		{
			bool isUserUnique = _userRepo.IsUniqueUser(model.Email);

			if (!isUserUnique)
			{
				return BadRequest(new { message = "User already exists" });
			}

			var user = await _userRepo.Register(model);
			if (user == null)
			{
				return BadRequest(new { message = "Error while register" });
			}

			return StatusCode(201);
		}
	}
}
