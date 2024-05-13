using Microsoft.AspNetCore.Mvc;
using Puppy.Repositories.Interfaces;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;

namespace Puppy.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FilesController : ControllerBase
{
    private readonly IWebHostEnvironment _environment;
    private readonly IFileRepository _fileRepo;

    public FilesController(IWebHostEnvironment environment, IFileRepository fileRepo)
    {
        _environment = environment;
        _fileRepo = fileRepo;
    }

    // GET
    [HttpGet("{fileName}")]
    public async Task<IActionResult> Index(string fileName)
    {
        using (var imageStream = await _fileRepo.GetFile(fileName))
        {
            using (var memoryStream = new MemoryStream())
            {
                await imageStream.CopyToAsync(memoryStream);
                memoryStream.Seek(0, SeekOrigin.Begin);

                using (var image = Image.Load(memoryStream))
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
        }
    }

    [HttpGet("{fileName}/small")]
    public async Task<IActionResult> Small(string fileName)
    {
        using (var imageStream = await _fileRepo.GetFile(fileName))
        {
            using (var memoryStream = new MemoryStream())
            {
                await imageStream.CopyToAsync(memoryStream);
                memoryStream.Seek(0, SeekOrigin.Begin);

                using (var image = Image.Load(memoryStream))
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
        }
    }
}