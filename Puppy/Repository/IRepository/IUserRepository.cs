using Curs.Models;
using Puppy.Models.Dto;

namespace Puppy.Repository.IRepository
{
	public interface IUserRepository
	{
		bool IsUniqueUser(string email, string username);
		Task<LoginResponseDto> Login(LoginRequestDto loginRequestDTO);
		Task<User> Register(RegistrationRequestDto registerRequestDTO);
	}
}
