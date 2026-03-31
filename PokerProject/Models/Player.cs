namespace PokerProject.Models
{
    public class Player
    {
        public int Id { get; set; }
        public int GameId { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTimeOffset? LeftAt { get; set; }
        public int RebuyCount { get; set; }
        public int ActiveBounties { get; set; }
        public Game Game { get; set; } = null!;
        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
