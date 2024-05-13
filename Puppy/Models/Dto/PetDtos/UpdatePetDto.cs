namespace Puppy.Models.Dto.PetDtos;

public class UpdatePetDto
{
    public string Name { get; set; }
    public string PassportNumber { get; set; }
    public List<IFormFile> Imgs { get; set; }
}