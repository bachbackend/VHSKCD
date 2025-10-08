using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using VHSKCD.DTOs.Auth;
using VHSKCD.Extension;
using VHSKCD.Models;
using VHSKCD.Repository;

namespace VHSKCD.Services.Impl
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepo;
        private readonly IConfiguration _configuration;

        public UserService(IUserRepository userRepo, IConfiguration configuration)
        {
            _userRepo = userRepo;
            _configuration = configuration;
        }

        public async Task AdminResetPasswordAsync(int userId, string newPassword)
        {
            var user = await _userRepo.GetByIdAsync(userId);
            if (user == null)
                throw new Exception("User not found.");

            user.Password = newPassword.Hash();
            await _userRepo.UpdateAsync(user);
        }

        public async Task ChangePasswordAsync(ChangePasswordDTO dto)
        {
            if (dto == null || string.IsNullOrEmpty(dto.NewPassword) || string.IsNullOrEmpty(dto.OldPassword))
                throw new Exception("Invalid request data.");

            if (dto.NewPassword == dto.OldPassword)
                throw new Exception("New password cannot equal with old password.");

            var user = await _userRepo.GetByIdAsync(dto.UserId);
            if (user == null)
                throw new Exception("User not found.");

            if (!dto.OldPassword.Verify(user.Password))
                throw new Exception("Wrong password.");

            user.Password = dto.NewPassword.Hash();
            await _userRepo.SaveChangesAsync();
        }

        public async Task<string> CreateAdminAsync(CreateAdminDTO dto)
        {
            var existingUser = await _userRepo.GetByUsernameAsync(dto.Username);
            if (existingUser != null)
                throw new Exception("Username already exists");

            var user = new User
            {
                Username = dto.Username,
                Password = dto.Password.Hash(),
                Email = dto.Email,
                Phonenumber = dto.Phonenumber,
                Role = 1,  // admin
                Status = 1,
                CreatedAt = DateTime.Now
            };

            await _userRepo.AddAsync(user);
            await _userRepo.SaveChangesAsync();

            return "Create manager account successfully.";
        }

        public async Task<string> CreateManagerAsync(CreateAdminDTO dto)
        {
            var existingUser = await _userRepo.GetByUsernameAsync(dto.Username);
            if (existingUser != null)
                throw new Exception("Username already exists");

            var user = new User
            {
                Username = dto.Username,
                Password = dto.Password.Hash(),
                Email = dto.Email,
                Phonenumber = dto.Phonenumber,
                Role = 2,  // manager
                Status = 1,
                CreatedAt = DateTime.Now
            };

            await _userRepo.AddAsync(user);
            await _userRepo.SaveChangesAsync();

            return "Create manager account successfully.";
        }

        public async Task<(string Token, User User)> LoginAsync(LoginDTO dto)
        {
            var user = await _userRepo.GetByUsernameAsync(dto.UserName);
            if (user == null)
                throw new Exception("Thông tin đăng nhập không hợp lệ.");

            if (!dto.Password.Verify(user.Password))
                throw new Exception("Thông tin đăng nhập không hợp lệ.");

            user.LastLogin = DateTime.Now;
            await _userRepo.SaveChangesAsync();

            // Generate JWT
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:SecretKey"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return (tokenString, user);
        }
    }
}
