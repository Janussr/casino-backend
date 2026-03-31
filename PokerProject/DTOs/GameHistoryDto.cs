namespace PokerProject.DTOs
{
    public class GameHistoryDto
    {
        public DateTimeOffset PlayedAt { get; set; }
        public required string Winner { get; set; }
        public int WinnerScore { get; set; }
        public required List<ScoreDto> Scores { get; set; }
    }
}
