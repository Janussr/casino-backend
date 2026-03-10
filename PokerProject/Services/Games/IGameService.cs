using PokerProject.DTOs;

namespace PokerProject.Services.Games
{
    public interface IGameService
    {
        Task<GameDto> StartGameAsync();
        Task<GameDto> EndGameAsync(int gameId);
        Task<GameDto> CancelGameAsync(int gameId);
        Task RemoveGameAsync(int gameId);
        Task<GameDto?> GetActiveGameAsync();
        Task<List<GameDto>> GetAllGamesAsync();
        Task<GameDto?> GetGameByIdAsync(int gameId);
        Task<GameDetailsDto?> GetGameDetailsAsync(int gameId, string? role);
        Task UpdateRulesAsync(int gameId, UpdateRulesDto dto);
    }
}
