namespace PokerProject.DTOs
{
    public class RoundDto
    {
        public int Id { get; set; }
        public int RoundNumber { get; set; }    
        public DateTimeOffset StartedAt { get; set; }
        public DateTimeOffset? EndedAt { get; set; }

        public List<ScoreDto> Scores { get; set; } = new();


    }
}
