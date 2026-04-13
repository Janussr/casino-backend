namespace PokerProject.DTOs.Players
{
    public class PlayerJoinedDto
    {
        public int GameId { get; set; }
        public PlayerDto Player { get; set; } = null!;
    }
}
