namespace PokerProject.DTOs.Bounties
{
    public class KnockoutAdminDto
    {
        public int GameId { get; set; }
        public int? KillerPlayerId { get; set; }
        public int? VictimPlayerId { get; set; }
    }
}
