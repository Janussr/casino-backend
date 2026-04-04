using PokerProject.DTOs.HallOfFame;

namespace PokerProject.Services.HallOfFames
{
    public interface IHallOfFameService
    {
        Task<List<HallOfFameDto>> GetEntireHallOfFameAsync();
    }
}
