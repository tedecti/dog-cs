using AutoMapper;
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
    private readonly IMapper _mapper;

    public FriendsController(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<ActionResult<string>> RemoveFriend(int id)
    {
        if (_context.Users == null)
        {
            return Problem("Entity set 'AppDbContext.Users'  is null.");
        }

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
        if (_context.Users == null)
        {
            return Problem("Entity set 'AppDbContext.Users'  is null.");
        }

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

    
    [HttpGet("{UserId}")]
    public async Task<ActionResult<IEnumerable<Friend>>> GetFriends(int UserId)
    {
        if (_context.Friend == null)
        {
            return Problem("Entity set 'AppDbContext.Friend' is null.");
        }

        var friends = await _context.Friend.Where(x => x.FollowerId == UserId).Include(x=>x.User)
            .ToListAsync();

        return Ok(_mapper.Map<IEnumerable<GetFollowersDto>>(friends));
    }
    
    [HttpGet("/api/followers/{UserId}")]
    public async Task<ActionResult<IEnumerable<Friend>>> GetFollowers(int UserId)
    {
        if (_context.Friend == null)
        {
            return Problem("Entity set 'AppDbContext.Friend' is null.");
        }

        var followers = await _context.Friend.Where(x => x.FollowerId == UserId).Include(x=>x.User).Include(x=>x.Follower)
            .ToListAsync();
        

        return Ok(_mapper.Map<IEnumerable<GetFollowersDto>>(followers));
    }

    [Authorize]
    [HttpGet("{UserId}/check")]
    public async Task<ActionResult<bool>> IsFriend(int UserId)
    {
        int currentUserId = Convert.ToInt32(User.Identity?.Name);

        if (_context.Friend == null)
        {
            return Problem("Entity set 'AppDbContext.Friend' is null.");
        }

        var follower = await _context.Friend.Where(x => x.FollowerId == currentUserId && x.UserId == UserId)
            .FirstOrDefaultAsync();

        if (follower == null) return false;

        return true;
    }
}