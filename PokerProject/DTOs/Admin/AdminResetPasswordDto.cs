namespace PokerProject.DTOs.Admin
{
    public class AdminResetPasswordDto
    {
        public int UserId { get; set; }
        public string NewPassword { get; set; } = null!;
    }
}
