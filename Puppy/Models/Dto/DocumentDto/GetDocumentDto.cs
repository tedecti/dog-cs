using Puppy.Models.Dto.PetDtos;

namespace Puppy.Models.Dto.DocumentDto;

public class GetDocumentDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string[] Imgs { get; set; }
    public DateTime UploadDate { get; set; }
    public ShortPetDto Pet { get; set; }
}