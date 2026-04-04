using PokerProject.DTOs.Players;

namespace PokerProject.Services.Players
{
    public interface IPlayerService
    {
        Task<List<PlayerDto>> AddPlayersToGameAsAdminAsync(int gameId, List<int> userIds);
        Task<List<PlayerDto>> GetPlayersAsync(int gameId);
        Task LeaveGameAsPlayerAsync(int gameId, int userId);
        Task RemovePlayerAsAdminAsync(int gameId, int playerId);
    }
}
