namespace PokerProject.DTOs
{
    public class GameDetailsDto
    {
        public int Id { get; set; }
        public int GameNumber { get; set; }
        public DateTimeOffset StartedAt { get; set; }
        public DateTimeOffset? EndedAt { get; set; }
        public bool IsFinished { get; set; }
        public Game.GameType Type { get; set; }
        public List<GameScoreboardDto> Scores { get; set; } = new();
        public List<RoundDto> Rounds { get; set; } = new();
        public List<PlayerDto> Players { get; set; }
        public WinnerDto? Winner { get; set; }
        public int? RebuyValue { get; set; }
        public int? BountyValue { get; set; }
    }

}
