namespace PokerProject.Models
{
    public class PokerHandAchievement
    {
        public int Id { get; set; }

        public int GameId { get; set; }
        public Game Game { get; set; } = null!;

        public int PlayerId { get; set; }
        public Player Player { get; set; } = null!;

        public DateTimeOffset AchievedAt { get; set; } = DateTimeOffset.UtcNow;

        public AchievementType Type { get; set; }

        public string? HoleCards { get; set; }
        public string? CommunityCards { get; set; }
        public string? Notes { get; set; }

        public enum AchievementType
        {
            RoyalFlush,
            BluffWithSevenTwo
        }
    }
}