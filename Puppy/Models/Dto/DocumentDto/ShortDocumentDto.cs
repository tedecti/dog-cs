namespace Puppy.Models.Dto.DocumentDto;

public class ShortDocumentDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string[] Imgs { get; set; }
    public DateTime UploadDate { get; set; }
}