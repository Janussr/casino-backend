using PokerProject.DTOs.Rounds;
using static Score;

namespace PokerProject.DTOs.Scores
{
    public class ScoreDto
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = null!;  
        public int Points { get; set; }
        public int GameId { get; set; }
        public RoundDto? Rounds { get; set; }
        public ScoreType Type { get; set; }

    }
}
