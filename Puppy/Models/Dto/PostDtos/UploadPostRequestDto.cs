using Curs.Models;

namespace Puppy.Models.Dto;

public class UploadPostRequestDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public List<string> Img { get; set; }
}