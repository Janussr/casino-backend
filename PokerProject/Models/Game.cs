using PokerProject.Models;

public class Game
{
    public int Id { get; set; }

    public int GameNumber { get; set; }
    public int GamemasterId { get; set; }
    public User Gamemaster { get; set; } = null;
    public int? RebuyValue { get; set; }
    public int? BountyValue { get; set; }
    public DateTimeOffset StartedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? EndedAt { get; set; }

    public bool IsOpenForPlayers { get; set; } = false;
    public bool IsFinished { get; set; }
    public int? WinnerPlayerId { get; set; }
    public Player? WinnerPlayer { get; set; }
    public string? JoinPasswordHash { get; set; }
    public ICollection<Player> Players { get; set; } = new List<Player>();
    public ICollection<Round> Rounds { get; set; } = new List<Round>();
    public ICollection<PokerHandAchievement> PokerHandAchievements { get; set; } = new List<PokerHandAchievement>();

    public GameType Type { get; set; }
    public enum GameType
    {
        BlackJack,
        Poker,
        Roulette
    }

} 
