using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Puppy.Models.Dto.AuthDtos;
using Puppy.Models.Dto.PostDtos;
using Puppy.Models.Dto.UserDtos;
using Puppy.Repositories.Interfaces;
using Puppy.Services.Interfaces;

namespace Puppy.Controllers;

[Route("api/admin")]
[ApiController]
public class AdminController : ControllerBase
{
    private readonly IAdminRepository _adminRepo;
    private readonly IAdminService _adminService;
    private readonly IMapper _mapper;

    public AdminController(IAdminRepository adminRepo, IAdminService adminService, IMapper mapper)
    {
        _mapper = mapper;
        _adminService = adminService;
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

    [HttpGet("users")]
    [Authorize]
    public async Task<IActionResult> GetTopUsers()
    {
        var role = _adminService.VerifyAdminRole(HttpContext.User.FindFirst(ClaimTypes.Role)?.Value);
        if (role == true)
        {
            var users = await _adminRepo.GetTopUsers();
            if (!users.Any())
            {
                return NotFound();
            }
            var userResponses = _mapper.Map<IEnumerable<UserResponseDto>>(users);

            return Ok(userResponses);
        }
        return Unauthorized("Your role is not supported");
    }
    
    [HttpGet("posts")]
    [Authorize]
    public async Task<IActionResult> GetPosts(int days)
    {
        var role = _adminService.VerifyAdminRole(HttpContext.User.FindFirst(ClaimTypes.Role)?.Value);
        if (role == true)
        {
            var posts = await _adminRepo.GetPosts(days);
            var response = _mapper.Map<IEnumerable<GetPostDto>>(posts);
            
            return Ok(response);
        }
        return Unauthorized("Your role is not supported");
    }
}