using PokerProject.DTOs.Players;
using PokerProject.DTOs.Rounds;
using PokerProject.DTOs.Scores;

namespace PokerProject.DTOs.Games
{
    public class GameDto
    {
        public int Id { get; set; }
        public int GameNumber { get; set; }
        public DateTimeOffset StartedAt { get; set; }
        public DateTimeOffset? EndedAt { get; set; }
        public bool IsFinished { get; set; }
        public Game.GameType Type { get; set; }

        public int? RebuyValue { get; set; }
        public int? BountyValue { get; set; }
        public List<PlayerDto> Players { get; set; } = new();
        public List<ScoreDto> Scores { get; set; } = new();
        public List<RoundDto> Rounds { get; set; } = [];

        public WinnerDto? Winner { get; set; }
    }
}
