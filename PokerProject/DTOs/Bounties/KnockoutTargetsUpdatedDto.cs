namespace PokerProject.DTOs.Bounties
{
    public class KnockoutTargetsUpdatedDto
    {
        public int GameId { get; set; }
        public List<KnockoutTargetDto> KnockoutTargets { get; set; } = new();
    }
}
