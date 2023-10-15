using Curs.Data;
using Curs.Models;
using Microsoft.IdentityModel.Tokens;
using Puppy.Models.Dto;
using Puppy.Repository.IRepository;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;

namespace Puppy.Repository
{
	public class UserRepository : IUserRepository
	{
		private readonly AppDbContext _context;
		private string secretKey;

		public UserRepository(AppDbContext context, IConfiguration configuration)
		{
			_context = context;
			secretKey = configuration.GetValue<string>("ApiSettings:Secret");
		}

		public bool IsUniqueUser(string email)
		{
			var user = _context.Users.FirstOrDefault(x => x.Email == email);
			if (user == null)
			{
				return true;
			}
			return false;
		}

		public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDTO)
		{
			var user = _context.Users.FirstOrDefault(u => u.Email == loginRequestDTO.Email && u.Password == loginRequestDTO.Password);

			if (user == null)
			{
				return new LoginResponseDto()
				{
					User = null,
					Token = ""
				}; ;
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
				User = user,
				Token = tokenHandler.WriteToken(token)
			};
			return loginResponseDTO;
		}

		public async Task<User> Register(RegistrationRequestDto registerRequestDTO)
		{
			User user = new User()
			{
				Email = registerRequestDTO.Email,
				FirstName = registerRequestDTO.FirstName,
				LastName = registerRequestDTO.LastName,
				Password = registerRequestDTO.Password,
			};
			_context.Users.Add(user);
			await _context.SaveChangesAsync();
			user.Password = "";
			return user;
		}
	}
}
