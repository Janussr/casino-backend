using PokerProject.DTOs.Admin;
using PokerProject.DTOs.Auth;
using PokerProject.Models;

namespace PokerProject.Services.Users
{
    public interface IUserService
    {
        Task<int?> GetActiveGameIdByUserAsync(int userId);
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDto?> GetUserByIdAsync(int id);
        Task<UserDto> RegisterAsync(RegisterUserDto dto);
        Task<User?> ValidateUserAsync(string username, string password);
        Task<UserDto?> AdminResetPasswordAsync(int userId, string newPassword);
        Task<UserDto?> PlayerUpdatePasswordAsync(int userId, string currentPassword, string newPassword);
        Task<UserDto?> SetUserRoleAsync(SetUserRoleDto dto);
        Task<UserDto?> PlayerUpdateUsernameAsync(int userId, string newUsername);
    }
}
