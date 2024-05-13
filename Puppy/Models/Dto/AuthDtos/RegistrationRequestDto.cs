using System.ComponentModel.DataAnnotations;

namespace Puppy.Models.Dto.AuthDtos
{
    public class RegistrationRequestDto
    {
        [Microsoft.Build.Framework.Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Microsoft.Build.Framework.Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Microsoft.Build.Framework.Required]
        [EmailAddress(ErrorMessage = "Поле 'Email' должно быть действительным адресом электронной почты.")]
        [StringLength(100)]
        public string Email { get; set; }

        [Microsoft.Build.Framework.Required]
        [StringLength(100)]
        public string Username { get; set; }

        [Microsoft.Build.Framework.Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; }
    }
}