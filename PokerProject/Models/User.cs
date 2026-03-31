namespace PokerProject.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;

        public UserRole Role { get; set; } = UserRole.User;
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

        public int CurrentWinStreak { get; set; } = 0;
        public int BestWinStreak { get; set; } = 0;
        public ICollection<HallOfFame> HallOfFames { get; set; } = new List<HallOfFame>();
        public ICollection<Player> Players { get; set; } = new List<Player>();


        public enum UserRole
        {
            User,
            Admin,
            Gamemaster
        }
    }

}
