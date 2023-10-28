using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;

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
        using (var imageStream = new FileStream(Path.Combine(_environment.ContentRootPath, "Images", $"{fileName}"), FileMode.Open))
        {
            using (var image = Image.Load(imageStream))
            {
                image.Mutate(x => x
                    .Resize(new ResizeOptions
                    {
                        Size = new Size(375, 500), 
                        Mode = ResizeMode.Max
                    })
                );

                var compressedImageStream = new MemoryStream();
                image.Save(compressedImageStream, new PngEncoder()); 
                compressedImageStream.Seek(0, SeekOrigin.Begin);

                return File(compressedImageStream, "image/png"); 
            }
        }


        
        var b = await System.IO.File.ReadAllBytesAsync(Path.Combine(_environment.ContentRootPath, "Images", $"{fileName}"));
        return File(b, "image/jpeg");;
    }    [HttpGet("{fileName}/small")]
    public async Task<IActionResult> Small(string fileName)
    {
        using (var imageStream = new FileStream(Path.Combine(_environment.ContentRootPath, "Images", $"{fileName}"), FileMode.Open))
        {
            using (var image = Image.Load(imageStream))
            {
                image.Mutate(x => x
                    .Resize(new ResizeOptions
                    {
                        Size = new Size(75, 100), 
                        Mode = ResizeMode.Max
                    })
                );

                var compressedImageStream = new MemoryStream();
                image.Save(compressedImageStream, new PngEncoder()); 
                compressedImageStream.Seek(0, SeekOrigin.Begin);

                return File(compressedImageStream, "image/png"); 
            }
        }


        
        var b = await System.IO.File.ReadAllBytesAsync(Path.Combine(_environment.ContentRootPath, "Images", $"{fileName}"));
        return File(b, "image/jpeg");;
    }
}