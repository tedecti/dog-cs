using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Puppy.Data;
using Puppy.Models;
using Puppy.Models.Dto.AdminDtos;
using Puppy.Models.Dto.AuthDtos;
using Puppy.Repositories.Interfaces;
using static System.DateTime;


namespace Puppy.Repositories;

public class AdminRepository : IAdminRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly string? _secretKey;
    public AdminRepository(AppDbContext context, IConfiguration configuration, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
        _secretKey = configuration.GetValue<string>("ApiSettings:Secret");
    }
            public async Task<AdminLoginResponseDto?> Login(LoginRequestDto loginRequestDto)
            {
                var admin = _context.Admin.FirstOrDefault(u =>
                    u.Email == loginRequestDto.Email);
    
                if (admin == null)
                {
                    return new AdminLoginResponseDto()
                    {
                        Admin = null,
                        Token = ""
                    };
                }
    
                var isVerified = BCrypt.Net.BCrypt.Verify(loginRequestDto.Password, admin.Password);
    
                if (!isVerified)
                {
                    return new AdminLoginResponseDto()
                    {
                        Admin = null,
                        Token = ""
                    };
                }
    
                var tokenHandler = new JwtSecurityTokenHandler();
                if (_secretKey == null) return null;
                var key = Encoding.ASCII.GetBytes(_secretKey);
    
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, admin.Id.ToString()),
                        new Claim(ClaimTypes.Role, "Admin"),
                    }),
                    Expires = UtcNow.AddDays(7),
                    SigningCredentials =
                        new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                };
    
                var token = tokenHandler.CreateToken(tokenDescriptor);
    
                var adminLoginResponseDto = new AdminLoginResponseDto()
                {
                    Admin = _mapper.Map<AdminDto>(admin),
                    Token = tokenHandler.WriteToken(token)
                };
                return adminLoginResponseDto;
            }
            
            public async Task<IEnumerable<Post>> GetPosts(int days)
            {
                var sevenDaysAgo = DateTime.UtcNow.AddDays(-(days));
                var posts = await _context.Post
                    .Where(x => x.UploadDate >= sevenDaysAgo)
                    .OrderByDescending(x => x.UploadDate)
                    .ToListAsync();
                return posts;
            }

            public async Task<IEnumerable<User>> GetTopUsers()
            {
                var users = await _context.Users
                    .Include(x => x.Followers)
                    .OrderByDescending(p => p.Followers.Count)
                    .ToListAsync();
                return users;
            }

}