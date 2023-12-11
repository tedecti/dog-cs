using Curs.Models;
using Puppy.Models.Dto;

namespace Puppy.Repository.IRepository
{
	public interface IUserRepository
	{
		Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);
		Task<User> Register(RegistrationRequestDto registerRequestDto);
		Task<User> Edit(UpdateUserDto updateUserDto, int userId);
		Task<User> UploadAvatar(int userId, string fName);
	}
}
