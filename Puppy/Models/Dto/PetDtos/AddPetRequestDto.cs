using System.ComponentModel.DataAnnotations;

namespace Puppy.Models.Dto.PetDtos
{
    public class AddPetRequestDto
    {
        [Microsoft.Build.Framework.Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Microsoft.Build.Framework.Required]
        [StringLength(100)]
        public string PassportNumber { get; set; }

        public List<IFormFile> Imgs { get; set; }
    }
}