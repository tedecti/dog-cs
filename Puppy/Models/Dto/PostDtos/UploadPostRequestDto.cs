namespace Puppy.Models.Dto;

public class UploadPostRequestDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public List<IFormFile> Img { get; set; }
}