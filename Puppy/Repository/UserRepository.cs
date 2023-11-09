using Curs.Data;
using Curs.Models;
using Microsoft.IdentityModel.Tokens;
using Puppy.Models.Dto;
using Puppy.Repository.IRepository;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Puppy.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private string secretKey;

        public UserRepository(AppDbContext context, IConfiguration configuration, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            secretKey = configuration.GetValue<string>("ApiSettings:Secret");
        }

        public bool IsUniqueEmail(string email)
        {
            var userEmail = _context.Users.FirstOrDefault(x => x.Email == email);
            return userEmail == null;
        }

        public bool IsUnique(string email, string username)
        {
            var user = _context.Users.FirstOrDefault(x => x.Email == email || x.Username == username);
            
            return user == null;
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

        public async Task<User> GetUser(int userId)
        {
            var user = await _context.Users.Include(x => x.Pets)
                .Include(x=>x.Posts)
                .Include(x=>x.Followers).ThenInclude(x=>x.Follower)
                .Include(x=>x.Friends).ThenInclude(x=>x.User)
                .FirstAsync(x => x.Id == Convert.ToInt32(userId));
            return user;
        }
    }
}