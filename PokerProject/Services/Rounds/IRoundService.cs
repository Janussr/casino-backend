using PokerProject.DTOs.Rounds;

namespace PokerProject.Services.Rounds
{
    public interface IRoundService
    {
        Task<RoundDto> StartNewRoundAsync(int gameId);
    }
}
