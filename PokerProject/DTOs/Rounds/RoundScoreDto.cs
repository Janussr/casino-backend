using PokerProject.DTOs.Scores;

namespace PokerProject.DTOs.Rounds
{
    public class RoundScoreDto
    {
        public int RoundId { get; set; }
        public int RoundNumber { get; set; }
        public DateTimeOffset StartedAt { get; set; }
        public int TotalPoints { get; set; }
        public List<ScoreEntryDto> Entries { get; set; } = new();
    }
}
