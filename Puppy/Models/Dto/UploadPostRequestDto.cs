using Curs.Models;

namespace Puppy.Models.Dto;

public class UploadPostRequestDto
{
    public string Description { get; set; }
    public Author Author { get; set; }
    public List<string> Img { get; set; }
    public DateTime UploadDate { get; set; }
}