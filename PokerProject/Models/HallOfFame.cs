namespace PokerProject.Models
{
    public class HallOfFame
    {
        public int Id { get; set; }
        public int GameId { get; set; }
        public Game Game { get; set; } = null!;
        public int PlayerId { get; set; }
        public Player Player { get; set; } = null!;
        public DateTimeOffset WinDate { get; set; } = DateTimeOffset.UtcNow;
    }
}