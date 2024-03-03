
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Puppy.Models.Dto;
using Puppy.Models.Dto.PostDtos;
using Puppy.Models.Dto.UserDtos;
using Puppy.Services.Interfaces;

namespace Puppy.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SearchController : ControllerBase
{
    private readonly ISearchService _searchService;
    private readonly IMapper _mapper;
    
    public SearchController(ISearchService searchService, IMapper mapper)
    {
        _searchService = searchService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> Search([FromQuery] string query)
    {
        var postResult = await _searchService.SearchPosts(query);
        var postDtos = _mapper.Map<IEnumerable<GetPostDto>>(postResult);
        var userResult = await _searchService.SearchUsers(query);
        var resultDtos = _mapper.Map<IEnumerable<ShortUserDto>>(userResult);
        var result = new
        {
            Posts = postDtos,
            Users = resultDtos
        };

        return Ok(result);
    }
}