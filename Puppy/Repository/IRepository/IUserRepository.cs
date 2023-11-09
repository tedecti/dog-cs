using Curs.Models;
using Puppy.Models.Dto;

namespace Puppy.Repository.IRepository
{
	public interface IUserRepository
	{
		bool IsUniqueEmail(string email);
		bool IsUnique (string email, string username);
		Task<LoginResponseDto> Login(LoginRequestDto loginRequestDTO);
		Task<User> Register(RegistrationRequestDto registerRequestDTO);
		Task<User> GetUser(int userId);
	}
}
