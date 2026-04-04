namespace PokerProject.DTOs.Bounties
{
    public class KnockoutTargetDto
    {
        public int PlayerId { get; set; }
        public string Username { get; set; } = null!;
        public int ActiveBounties { get; set; }
    }
}
