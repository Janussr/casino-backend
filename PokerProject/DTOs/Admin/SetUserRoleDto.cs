using PokerProject.Models;

namespace PokerProject.DTOs.Admin
{
    public class SetUserRoleDto
    {
        public int UserId { get; set; }
        public User.UserRole Role { get; set; }
    }
}
