using PokerProject.DTOs;

namespace PokerProject.Services.Bounties
{
    public interface IBountyService
    {
        Task RegisterKnockoutAsync(int gameId, int killerUserId, int victimUserId, bool isAdmin);
        Task<List<BountyLeaderboardDto>> GetBountyLeaderboardAsync();
    }
}
