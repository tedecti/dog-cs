using Curs.Models;
using Puppy.Models.Dto;

namespace Puppy.Repository.IRepository
{
	public interface IUserRepository
	{
		Task<LoginResponseDto> Login(LoginRequestDto loginRequestDTO);
		Task<User> Register(RegistrationRequestDto registerRequestDTO);
	}
}
