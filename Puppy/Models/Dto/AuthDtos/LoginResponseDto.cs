using Curs.Models;

namespace Puppy.Models.Dto
{
	public class LoginResponseDto
	{
		public UserResponseDto User { get; set; }
		public string Token { get; set; }
	}
}
