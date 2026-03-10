using PokerProject.DTOs;

namespace PokerProject.Services.Participants
{
    public interface IParticipantService
    {

        Task AddParticipantsAsync(int gameId, List<int> userIds);
        Task<List<ParticipantDto>> GetParticipantsAsync(int gameId);
        Task<bool> IsUserParticipantAsync(int gameId, int userId);
        Task<List<ParticipantDto>> RemoveParticipantAsync(int gameId, int userId);
    }
}
