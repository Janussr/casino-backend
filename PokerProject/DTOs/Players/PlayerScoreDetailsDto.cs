using PokerProject.DTOs.Rounds;

namespace PokerProject.DTOs.Players
{
    public class PlayerScoreDetailsDto
    {
        public int UserId { get; set; }
        public int PlayerId { get; set; }
        public string UserName { get; set; } = "";
        public int TotalPoints { get; set; }
        public List<RoundScoreDto> Rounds { get; set; } = new();
    }
}
