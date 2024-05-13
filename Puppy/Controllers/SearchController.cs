using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Puppy.Models.Dto;
using Puppy.Models.Dto.PostDtos;
using Puppy.Models.Dto.UserDtos;
using Puppy.Repositories.Interfaces;
using Puppy.Services.Interfaces;

namespace Puppy.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SearchController : ControllerBase
{
    private readonly ISearchRepository _searchRepository;
    private readonly IMapper _mapper;

    public SearchController(IMapper mapper, ISearchRepository searchRepository)
    {
        _mapper = mapper;
        _searchRepository = searchRepository;
    }

    [HttpGet]
    public async Task<IActionResult> Search([FromQuery] string query)
    {
        var postResult = await _searchRepository.SearchPosts(query);
        var postDtos = _mapper.Map<IEnumerable<GetPostDto>>(postResult);
        var userResult = await _searchRepository.SearchUsers(query);
        var resultDtos = _mapper.Map<IEnumerable<ShortUserDto>>(userResult);
        var result = new
        {
            Posts = postDtos,
            Users = resultDtos
        };

        return Ok(result);
    }
}