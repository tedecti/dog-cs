using System.Collections;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Puppy.Data;
using Puppy.Models;
using Puppy.Models.Dto;
using Puppy.Models.Dto.FollowerDtos;
using Puppy.Services.Interfaces;

namespace Puppy.Controllers;

[Route("api/friends")]
[ApiController]
public class FriendsAndFollowersController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IFriendService _friendService;
    private readonly IMapper _mapper;

    public FriendsAndFollowersController(AppDbContext context, IMapper mapper, IFriendService friendService)
    {
        _context = context;
        _mapper = mapper;
        _friendService = friendService;
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<ActionResult<string>> RemoveFriend(int id)
    {
        var userId = HttpContext.User.Identity?.Name;

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var existingFriend = await _context.Friend.FirstOrDefaultAsync(
            f => f.FollowerId.ToString() == userId && f.UserId == id);

        if (existingFriend == null)
        {
            return BadRequest("You are not followed");
        }

        _context.Friend.Remove(existingFriend);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [Authorize]
    [HttpPost("{id}")]
    public async Task<ActionResult<string>> AddFriend(int id)
    {

        var userId = HttpContext.User.Identity?.Name;

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var existingFriend = await _context.Friend.FirstOrDefaultAsync(
            f => f.FollowerId.ToString() == userId && f.UserId == id);
        if (existingFriend != null)
        {
            return BadRequest("You already followed");
        }

        if (userId == id.ToString())
        {
            return BadRequest("You can't follow yourself");
        }

        var newFriend = new Friend()
        {
            UserId = id,
            FollowerId = Convert.ToInt32(userId)
        };

        _context.Friend.Add(newFriend);
        await _context.SaveChangesAsync();

        return StatusCode(201);
    }

    
    [HttpGet("{userId}")]
    public async Task<IActionResult> GetFriends(int userId)
    {
        var friends = await _friendService.GetFriends(userId);
        var response = _mapper.Map<IEnumerable<GetFollowersDto>>(friends);
        return Ok(response);
        
    }

    [Authorize]
    [HttpGet("{userId}/check")]
    public async Task<ActionResult<bool>> IsFriend(int userId)
    {
        var currentUserId = Convert.ToInt32(User.Identity?.Name);

        var follower = await _friendService.IsFriend(userId, currentUserId);
        
        return follower;
    }
}