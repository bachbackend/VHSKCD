using VHSKCD.DTOs.Auth;
using VHSKCD.Models;

namespace VHSKCD.Services
{
    public interface IUserService
    {
        Task<string> CreateManagerAsync(CreateAdminDTO dto);
        Task<string> CreateAdminAsync(CreateAdminDTO dto);
        Task<(string Token, User User)> LoginAsync(LoginDTO dto);
        Task ChangePasswordAsync(ChangePasswordDTO dto);
        Task AdminResetPasswordAsync(int userId, string newPassword);
    }
}
