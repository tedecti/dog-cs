using Microsoft.AspNetCore.Mvc;

namespace Puppy.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FilesController : ControllerBase
{

    private readonly IWebHostEnvironment _environment;

    public FilesController(IWebHostEnvironment environment)
    {
        _environment = environment;
    }
    
    // GET
    [HttpGet("{fileName}")]
    public async Task<IActionResult> Index(string fileName)
    {
        var b = await System.IO.File.ReadAllBytesAsync(Path.Combine(_environment.ContentRootPath, "Images", $"{fileName}"));
        return File(b, "image/jpeg");;
    }
}