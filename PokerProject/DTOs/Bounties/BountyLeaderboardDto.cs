namespace PokerProject.DTOs.Bounties
{
    public class BountyLeaderboardDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = "";
        public int Knockouts { get; set; }
        public int TimesKnockedOut { get; set; }
        public int TotalBountyPoints { get; set; }
    }
}
