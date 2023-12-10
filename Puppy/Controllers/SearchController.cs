
using AutoMapper;
using Curs.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Puppy.Data;
using Puppy.Models.Dto;

namespace Puppy.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SearchController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    
    public SearchController(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    async public Task<IActionResult> Search([FromQuery] string query)
    {
        var postResult = await _context.Post.Include(x=>x.User).Where(p =>
            p.Title.ToLower().Contains(query.ToLower()))
            .ToListAsync();

        var postDtos = _mapper.Map<IEnumerable<GetPostDto>>(postResult);

        var userResult = await _context.Users.Where(u => u.Username.ToLower().Contains(query.ToLower()))
            .ToListAsync();

        var resultDtos = _mapper.Map<IEnumerable<ShortUserDto>>(userResult);
        
        var result = new
        {
            Posts = postDtos,
            Users = resultDtos
        };

        return Ok(result);
    }
}