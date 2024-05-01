using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Puppy.Data;
using Puppy.Models.Dto.FollowerDtos;
using Puppy.Repository.Interfaces;
using Puppy.Services.Interfaces;

namespace Puppy.Controllers;

[Route("api/followers")]
[ApiController]
public class FollowersController : ControllerBase
{
    private readonly IFollowerService _followerService;
    private readonly IFollowerRepository _followerRepository;
    private readonly IMapper _mapper;

    public FollowersController(IMapper mapper, IFollowerService followerService, IFollowerRepository followerRepository)
    {
        _mapper = mapper;
        _followerService = followerService;
        _followerRepository = followerRepository;
    }

    [Authorize]
    [HttpPost("{id}")]
    public async Task<ActionResult<string>> Follow(int id)
    {

        var userId = HttpContext.User.Identity?.Name;

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var existingFollow = await _followerService.IsFollowed(id, Convert.ToInt32(userId));

        if (existingFollow)
        {
            return BadRequest("You already followed this person");
        }
        
        if (userId == id.ToString())
        {
            return BadRequest("You can't follow yourself");
        }

        await _followerRepository.Follow(id, Convert.ToInt32(userId));

        return StatusCode(201);
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<ActionResult<string>> RemoveFollow(int id)
    {
        var userId = HttpContext.User.Identity?.Name;

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var existingFriend = await _followerService.IsFollowed(id, Convert.ToInt32(userId));

        if (existingFriend == false)
        {
            return BadRequest("You are not followed");
        }

        await _followerRepository.Unfollow(id, Convert.ToInt32(userId));
        return NoContent();
    }
    
    [HttpGet("{userId}")]
    public async Task<IActionResult> GetFollowers(int userId)
    {
        var friends = await _followerService.GetFollowers(userId);
        var response = _mapper.Map<IEnumerable<GetFollowersDto>>(friends);
        return Ok(response);
        
    }

    [Authorize]
    [HttpGet("{userId}/check")]
    public async Task<ActionResult<bool>> IsFollowed(int userId)
    {
        var currentUserId = Convert.ToInt32(User.Identity?.Name);

        var follower = await _followerService.IsFollowed(userId, currentUserId);
        
        return follower;
    }
}