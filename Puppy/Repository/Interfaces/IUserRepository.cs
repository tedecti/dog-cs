using Puppy.Models;
using Puppy.Models.Dto.AuthDtos;
using Puppy.Models.Dto.UserDtos;

namespace Puppy.Repository.Interfaces
{
	public interface IUserRepository
	{
		Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);
		Task<User> Register(RegistrationRequestDto registerRequestDto);
		Task<User> Edit(UpdateUserDto updateUserDto, int userId);
		Task<User> UploadAvatar(int userId, string fName);
	}
}
