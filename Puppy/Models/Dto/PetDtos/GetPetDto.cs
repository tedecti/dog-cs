namespace Puppy.Models.Dto;

public class GetPetDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string PassportNumber { get; set; }
    public string[] Imgs { get; set; }
    public ShortUserDto User { get; set; }   
}