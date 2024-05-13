namespace Puppy.Models.Dto.DocumentDto;

public class UpdateDocumentDto
{
    public string Title { get; set; }
    public string Description { get; set; }

    public List<IFormFile> Imgs { get; set; }
}