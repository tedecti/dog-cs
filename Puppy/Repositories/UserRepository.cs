using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Puppy.Data;
using Puppy.Models;
using Puppy.Models.Dto.AuthDtos;
using Puppy.Models.Dto.ChatDto;
using Puppy.Models.Dto.UserDtos;
using Puppy.Repositories.Interfaces;
using Puppy.Services.Interfaces;
using static System.DateTime;

namespace Puppy.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly string? _secretKey;
        private readonly IChatRepository _chatRepository;

        public UserRepository(AppDbContext context, IConfiguration configuration, IMapper mapper, IChatRepository chatRepository)
        {
            _context = context;
            _mapper = mapper;
            _secretKey = configuration.GetValue<string>("ApiSettings:Secret");
            _chatRepository = chatRepository;
        }

        public async Task<LoginResponseDto?> Login(LoginRequestDto loginRequestDto)
        {
            var user = _context.Users.FirstOrDefault(u =>
                u.Email == loginRequestDto.Email);

            if (user == null)
            {
                return new LoginResponseDto()
                {
                    User = null,
                    Token = ""
                };
            }

            var isVerified = BCrypt.Net.BCrypt.Verify(loginRequestDto.Password, user.Password);

            if (!isVerified)
            {
                return new LoginResponseDto()
                {
                    User = null,
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
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, "User"),
                }),
                Expires = UtcNow.AddDays(7),
                SigningCredentials =
                    new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            var loginResponseDto = new LoginResponseDto()
            {
                User = _mapper.Map<UserResponseDto>(user),
                Token = tokenHandler.WriteToken(token)
            };
            return loginResponseDto;
        }

        public async Task<User> Register(RegistrationRequestDto registerRequestDto)
        {
            string password = BCrypt.Net.BCrypt.HashPassword(registerRequestDto.Password, 12);

            var user = new User()
            {
                Email = registerRequestDto.Email,
                Avatar = "",
                FirstName = registerRequestDto.FirstName,
                Username = registerRequestDto.Username,
                LastName = registerRequestDto.LastName,
                Password = password,
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            user.Password = "";
            await _chatRepository.CreateRoom(0, user.Id);
            var registeredUserChats = await _chatRepository.GetRoomsByUser(user.Id);
            var message = new SendMessageDto()
            {
                Message = "Добро пожаловать в наше приложение PetPaw"
            };
            var roomId = registeredUserChats[0].RoomId;
            if (roomId != null)
                await _chatRepository.CreateMessage(roomId, 0, message);
            return user;
        }

        public async Task<User?> Edit(UpdateUserDto updateUserDto, int userId)
        {
            var existingUser = await GetUser(userId);
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
            var user = await GetUser(userId);
            user.Avatar = fName;
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> GetUser(int userId)
        {
            var user = await _context.Users.Include(x => x.Pets)
                .Include(x => x.Posts)
                .Include(x => x.Followers).ThenInclude(x => x.Follower)
                .Include(x => x.Friends).ThenInclude(x => x.User)
                .FirstAsync(x => x.Id == Convert.ToInt32(userId));
            return user;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            var users = await _context.Users.ToListAsync();
            return users;
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
    }
}