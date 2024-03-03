using Microsoft.IdentityModel.Tokens;
using Puppy.Models.Dto;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Puppy.Data;
using Puppy.Models;
using Puppy.Models.Dto.AuthDtos;
using Puppy.Models.Dto.UserDtos;
using Puppy.Repository.Interfaces;
using Puppy.Services.Interfaces;

namespace Puppy.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private string secretKey;

        public UserRepository(AppDbContext context, IConfiguration configuration, IMapper mapper, IUserService service)
        {
            _context = context;
            _mapper = mapper;
            secretKey = configuration.GetValue<string>("ApiSettings:Secret");
            _userService = service;
        }
        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDTO)
        {
            
            var user = _context.Users.FirstOrDefault(u =>
                u.Email == loginRequestDTO.Email );

            if (user == null)
            {
                return new LoginResponseDto()
                {
                    User = null,
                    Token = ""
                };
            }

            
            bool isVerified = BCrypt.Net.BCrypt.Verify(loginRequestDTO.Password, user.Password);
            
            if (!isVerified)
            {
                return new LoginResponseDto()
                {
                    User = null,
                    Token = ""
                };
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, "User"),
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            LoginResponseDto loginResponseDTO = new LoginResponseDto()
            {
                User = _mapper.Map<UserResponseDto>(user),
                Token = tokenHandler.WriteToken(token)
            };
            return loginResponseDTO;
        }

        public async Task<User> Register(RegistrationRequestDto registerRequestDTO)
        {
            string password = BCrypt.Net.BCrypt.HashPassword(registerRequestDTO.Password, 12);

            User user = new User()
            {
                Email = registerRequestDTO.Email,
                Avatar = "",
                FirstName = registerRequestDTO.FirstName,
                Username = registerRequestDTO.Username,
                LastName = registerRequestDTO.LastName,
                Password = password,
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            user.Password = "";
            return user;
        }

        public async Task<User> Edit(UpdateUserDto updateUserDto, int userId)
        {
            var existingUser = await _userService.GetUser(userId);
            existingUser.FirstName = updateUserDto.FirstName;
            existingUser.LastName = updateUserDto.LastName;
            if (existingUser == null)
            {
                return null;
            }
            await _context.SaveChangesAsync();
            return existingUser;
        }

        public async Task<User> UploadAvatar(int userId, string fName)
        {
            var user = await _userService.GetUser(userId);
            user.Avatar = fName;
            await _context.SaveChangesAsync();
            return user;
        }
    }
}