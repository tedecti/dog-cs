using Puppy.Models.Dto.UserDtos;

namespace Puppy.Models.Dto.AuthDtos
{
	public class LoginResponseDto
	{
		public UserResponseDto User { get; set; }
		public string Token { get; set; }
	}
}
