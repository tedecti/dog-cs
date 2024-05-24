using Microsoft.AspNetCore.Mvc;
using Puppy.Models.Dto.AuthDtos;
using Puppy.Repositories.Interfaces;

namespace Puppy.Controllers;

[Route("api/admin")]
[ApiController]
public class AdminController : ControllerBase
{
    private readonly IAdminRepository _adminRepo;

    public AdminController(IAdminRepository adminRepo)
    {
        _adminRepo = adminRepo;
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto model)
    {
        var response = await _adminRepo.Login(model);

        if (response?.Admin == null || string.IsNullOrEmpty(response.Token))
        {
            return BadRequest(new { message = "Email or password incorrect" });
        }

        return StatusCode(200, response);
    }
}