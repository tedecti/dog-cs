namespace Puppy.Models.Dto;

public class UserPetDto
{
        public int Id { get; set; }
        public string Name { get; set; }
        public string PassportNumber { get; set; }
        public string[] Imgs { get; set; }
}