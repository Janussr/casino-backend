using PokerProject.DTOs;

namespace PokerProject.Services.HallOfFames
{
    public interface IHallOfFameService
    {
        Task<List<HallOfFameDto>> GetEntireHallOfFameAsync();
    }
}
