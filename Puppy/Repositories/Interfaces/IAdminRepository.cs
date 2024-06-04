using Puppy.Models;
using Puppy.Models.Dto.AdminDtos;
using Puppy.Models.Dto.AuthDtos;

namespace Puppy.Repositories.Interfaces;

public interface IAdminRepository
{
    Task<AdminLoginResponseDto> Login(LoginRequestDto loginRequestDto);
    Task<IEnumerable<User>> GetTopUsers();
    Task<IEnumerable<Post>> GetPosts(int days);
}