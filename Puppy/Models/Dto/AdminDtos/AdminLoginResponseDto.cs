namespace Puppy.Models.Dto.AdminDtos;

public class AdminLoginResponseDto
{
    public AdminDto? Admin { get; set; }
    public string Token { get; set; }
}