namespace PokerProject.DTOs.Players
{
    public class PlayerRemovedDto
    {
        public int GameId { get; set; }
        public int RemovedPlayerId { get; set; }
        public int? RemovedUserId { get; set; }
    }
}
