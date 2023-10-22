using Curs.Data;
using Curs.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Puppy.Models.Dto;

namespace Puppy.Controllers;

[Route("api/friends")]
[ApiController]
public class FriendsController : ControllerBase
{
    
    private readonly AppDbContext _context;

    public FriendsController(AppDbContext context)
    {
        _context = context;
    }
    
    [Authorize]
    [HttpPost("{id}")]
    public async Task<ActionResult<string>> AddFriend(int id)
    {
        if (_context.Users == null)
        {
            return Problem("Entity set 'AppDbContext.Users'  is null.");
        }

        var userId = HttpContext.User.Identity.Name;
        var existingFriend = await _context.Friend.FirstOrDefaultAsync(f => f.UserId.ToString() == userId && f.FollowerId == id);
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        if (existingFriend != null)
        {
            return BadRequest("You already followed");
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

    [HttpGet("{UserId}")]
    public async Task<ActionResult<IEnumerable<Friend>>> GetLike(int UserId)
    {
        UserId = Convert.ToInt32(User.Identity.Name);
        var followers = await _context.Friend
            .ToListAsync();

        if (_context.Friend == null)
        {
            return Problem("Entity set 'AppDbContext.Friend' is null.");
        }
        var followersDto = followers.Select(follower => new GetFollowersDto()
        {
            UserId = follower.UserId,
            FollowerId = follower.FollowerId,
        });
        
        return Ok(followersDto);
    }
}