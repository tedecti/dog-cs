using Curs.Data;
using Microsoft.AspNetCore.Mvc;

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

    [HttpPost("add")]
    public IActionResult AddFriend(int userId, int friendId)
    {
        var user = _context.Users.FirstOrDefault(u => u.Id == userId);
        var friend = _context.Users.FirstOrDefault(u => u.Id == friendId);

        if (user == null || friend == null)
        {
            return BadRequest("User(s) not found");
        }

        user.Friends.Add(friend);
        friend.Friends.Add(user);

        _context.SaveChanges();

        return Ok("Friend added successfully");
    }
}
