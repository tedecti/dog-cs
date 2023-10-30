
using Curs.Data;
using Microsoft.AspNetCore.Mvc;
using Puppy.Models.Dto;

namespace Puppy.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SearchController : ControllerBase
{
    private readonly AppDbContext _context;
    
    public SearchController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Search([FromQuery] string query)
    {
        var postResult = _context.Post.Where(p =>
            p.Title.ToLower().Contains(query.ToLower()))
            .Select(p => new GetPostDto
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                Imgs = p.Imgs,
                UploadDate = p.UploadDate
            })
            .ToList();

        var userResult = _context.Users.Where(u => u.Username.ToLower().Contains(query.ToLower()))
            .Select(u=>new ShortUserDto
            {
                Id = u.Id,
                Avatar = u.Avatar,
                Username = u.Username,
                FirstName = u.FirstName,
                LastName = u.LastName
            })
            .ToList();

        var result = new
        {
            Posts = postResult,
            Users = userResult
        };

        return Ok(result);
    }
}