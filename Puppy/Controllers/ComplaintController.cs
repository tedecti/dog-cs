using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Puppy.Models;
using Puppy.Models.Dto.ComplaintDtos;
using Puppy.Models.Dto.PostDtos;
using Puppy.Models.Dto.UserDtos;
using Puppy.Repositories.Interfaces;
using Puppy.Services.Interfaces;

namespace Puppy.Controllers;
[Route("api/complaint")]
public class ComplaintController : ControllerBase
{
    private readonly IComplaintRepository _repository;
    private readonly IMapper _mapper;
    private readonly IAdminService _adminService;

    public ComplaintController(IComplaintRepository repository, IMapper mapper, IAdminService adminService)
    {
        _repository = repository;
        _mapper = mapper;
        _adminService = adminService;
    }

    [HttpGet("user/{UserId}")]
    [Authorize]
    public async Task<ActionResult<UserComplaintsDto>> GetUserComplaints(int UserId)
    {
        var role = _adminService.VerifyAdminRole(HttpContext.User.FindFirst(ClaimTypes.Role)?.Value);
        if (role == true)
        {
            var complaints = await _repository.GetUserComplaints(UserId);
            if (complaints == null)
            {
                return NotFound();
            }

            var response = _mapper.Map<IEnumerable<GetComplaintDto>>(complaints);
            return Ok(response);
        }
        return Unauthorized("Your role is not supported");
    }

    [HttpGet("{Id}")]
    [Authorize]
    public async Task<ActionResult<UserComplaintsDto>> GetComplaint(int Id)
    {
        var role = _adminService.VerifyAdminRole(HttpContext.User.FindFirst(ClaimTypes.Role)?.Value);
        if (role == true)
        {
            var complaints = await _repository.GetComplaint(Id);
            if (complaints == null)
            {
                return NotFound();
            }

            var response = _mapper.Map<GetComplaintDto>(complaints);
            return Ok(response);
        }
        return Unauthorized("Your role is not supported");
    }
    
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<GetComplaintDto>>> GetComplaints()
    {
        var role = _adminService.VerifyAdminRole(HttpContext.User.FindFirst(ClaimTypes.Role)?.Value);
        if (role == true)
        {
            var complaints = await _repository.GetComplaints();
            if (complaints == null)
            {
                return NotFound();
            }

            var response = _mapper.Map<IEnumerable<GetComplaintDto>>(complaints);
            return Ok(response);
        }
        return Unauthorized("Your role is not supported");
    }
    
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<Complaint>> PostComplaint(int postId, ComplaintRequestDto complaintRequestDto)
    {
        var role = _adminService.VerifyAdminRole(HttpContext.User.FindFirst(ClaimTypes.Role)?.Value);
        if (role == false)
        {
            var userId = Convert.ToInt32(HttpContext.User.Identity?.Name);

            var complaint = await _repository.CreateComplaint(complaintRequestDto, userId, postId);
            if (complaint == null)
            {
                return BadRequest("Unable to create complaint");
            }

            return StatusCode(201);
        }
        return Unauthorized("Your role is not supported");
    }

    [HttpGet("post/{postId}")]
    [Authorize]
    public async Task<ActionResult<PostComplaintsDto>> GetPostComplaints(int postId)
    {
        var role = _adminService.VerifyAdminRole(HttpContext.User.FindFirst(ClaimTypes.Role)?.Value);
        if (role == true)
        {
            var complaints = await _repository.GetPostComplaints(postId);
            if (complaints == null)
            {
                return BadRequest("Unable to get complaints");
            }

            var response = _mapper.Map<PostComplaintsDto>(complaints);
            return Ok(response);
        }
        return Unauthorized("Your role is not supported");
    }

    [HttpGet("users")]
    [Authorize]
    public async Task<IActionResult> GetUsersComplaints()
    {
        var role = _adminService.VerifyAdminRole(HttpContext.User.FindFirst(ClaimTypes.Role)?.Value);
        if (role == true)
        {
            var users = await _repository.GetUsers();
            if (!users.Any())
            {
                return NotFound();
            }

            var response = _mapper.Map<IEnumerable<UserComplaintsDto>>(users);
            return Ok(response);
        }
        return Unauthorized("Your role is not supported");
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> EditStatus(ComplaintEditDto complaintEditDto, int id)
    {
        var role = _adminService.VerifyAdminRole(HttpContext.User.FindFirst(ClaimTypes.Role)?.Value);
        if (role == true)
        {
            var complaint = await _repository.GetComplaint(id);
            if (complaint == null)
            {
                return BadRequest("Complaint not found");
            }

            await _repository.SetStatus(complaintEditDto, id);
            return NoContent();
        }
        return Unauthorized("Your role is not supported");
    }
}