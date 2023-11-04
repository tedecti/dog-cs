namespace Curs.Models.Dto.DocumentDto;

public class UploadDocumentDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    
    public List<IFormFile> Imgs { get; set; }
}