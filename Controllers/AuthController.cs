using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VHSKCD.DTOs.Auth;
using VHSKCD.Services;

namespace VHSKCD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("CreateManager")]
        public async Task<IActionResult> CreateManager([FromBody] CreateAdminDTO dto)
        {
            try
            {
                var result = await _userService.CreateManagerAsync(dto);
                return Ok(new { message = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("CreateAdmin")]
        public async Task<IActionResult> CreateAdmin([FromBody] CreateAdminDTO dto)
        {
            try
            {
                var result = await _userService.CreateAdminAsync(dto);
                return Ok(new { message = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            try
            {
                var (token, user) = await _userService.LoginAsync(dto);
                return Ok(new
                {
                    token,
                    userId = user.Id,
                    username = user.Username,
                    role = user.Role
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO dto)
        {
            try
            {
                await _userService.ChangePasswordAsync(dto);
                return Ok(new { message = "Password updated successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("AdminResetPassword/{userId}")]
        public async Task<IActionResult> AdminResetPassword(int userId, [FromBody] string newPassword)
        {
            try
            {
                await _userService.AdminResetPasswordAsync(userId, newPassword);
                return Ok(new { message = "Password has been reset successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

    }
}
