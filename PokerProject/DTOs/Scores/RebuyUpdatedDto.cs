namespace PokerProject.DTOs.Scores
{
    public class RebuyUpdatedDto
    {
        public int GameId { get; set; }
        public int PlayerId { get; set; }
        public int RebuyCount { get; set; }
        public ScoreDto Score { get; set; } = null!;
    }
}
