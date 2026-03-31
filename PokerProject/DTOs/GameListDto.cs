namespace PokerProject.DTOs
{
    public class GameListDto
    {
        public int Id { get; set; }
        public int GameNumber { get; set; }
        public Game.GameType Type { get; set; }
        public DateTimeOffset StartedAt { get; set; }
        public bool IsFinished { get; set; }

        public int PlayerCount { get; set; }
    }
}
