using Microsoft.EntityFrameworkCore;
using PokerProject.Data;
using PokerProject.DTOs.Bounties;
using PokerProject.DTOs.Players;
using PokerProject.Hubs.GameNotifier;
using PokerProject.Models;

namespace PokerProject.Services.Players
{
    public class PlayerService : IPlayerService
    {
        private readonly PokerDbContext _context;
        private readonly IGameNotifier _gameNotifier;

        public PlayerService(PokerDbContext context, IGameNotifier gameNotifier)
        {
            _context = context;
            _gameNotifier = gameNotifier;
        }


        public async Task<List<PlayerDto>> AddPlayersToGameAsAdminAsync(int gameId, List<int> userIds)
        {
            var game = await _context.Games
                .Include(g => g.Players)
                    .ThenInclude(p => p.User)
                .FirstOrDefaultAsync(g => g.Id == gameId);

            if (game == null)
                throw new KeyNotFoundException("Game not found");

            var players = new List<Player>();

            foreach (var userId in userIds)
            {
                var existingPlayer = game.Players.FirstOrDefault(p => p.UserId == userId);

                if (existingPlayer != null)
                {
                    if (!existingPlayer.IsActive)
                    {
                        existingPlayer.IsActive = true;
                        existingPlayer.LeftAt = null;
                        players.Add(existingPlayer);
                    }

                    continue;
                }

                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                    continue;

                var player = new Player
                {
                    GameId = gameId,
                    UserId = userId,
                    IsActive = true,
                    RebuyCount = 0,
                    ActiveBounties = 0,
                    User = user
                };

                game.Players.Add(player);
                players.Add(player);
            }

            await _context.SaveChangesAsync();

            var result = players.Select(player => new PlayerDto
            {
                PlayerId = player.Id,
                UserId = player.UserId,
                Username = player.User?.Username ?? "Unknown",
                RebuyCount = player.RebuyCount,
                ActiveBounties = player.ActiveBounties,
                IsActive = player.IsActive
            }).ToList();

            var knockoutTargets = game.Players
                .Where(p => p.IsActive)
                .Select(p => new KnockoutTargetDto
                {
                    PlayerId = p.Id,
                    Username = p.User?.Username ?? "Unknown",
                    ActiveBounties = p.ActiveBounties
                })
                .ToList();

            await _gameNotifier.KnockoutTargetsUpdated(game.Id, knockoutTargets);

            return result;
        }

        public async Task<List<PlayerDto>> GetPlayersAsync(int gameId)
        {
            return await _context.Players
                .AsNoTracking()
                .Where(gp => gp.GameId == gameId)
                .Include(gp => gp.User)
                .Select(gp => new PlayerDto
                {
                    UserId = gp.UserId,
                    Username = gp.User.Username
                })
                .ToListAsync();
        }

        public async Task<bool> IsUserAPlayerAsync(int gameId, int userId)
        {
            return await _context.Players
                .AsNoTracking()
                .AnyAsync(gp => gp.GameId == gameId && gp.UserId == userId);
        }


        public async Task LeaveGameAsPlayerAsync(int gameId, int userId)
        {
            var game = await _context.Games
                .Include(g => g.Players)
                    .ThenInclude(p => p.User)
                .FirstOrDefaultAsync(g => g.Id == gameId);

            if (game == null)
                throw new KeyNotFoundException("Game not found");

            if (game.IsFinished)
                throw new Exception("Game already finished");

            var player = game.Players.FirstOrDefault(p => p.UserId == userId);

            if (player == null)
                throw new Exception("Player not found");

            if (!player.IsActive)
                return;

            player.IsActive = false;
            player.LeftAt = DateTimeOffset.UtcNow;

            await _context.SaveChangesAsync();

            var knockoutTargets = game.Players
                .Where(p => p.IsActive)
                .Select(p => new KnockoutTargetDto
                {
                    PlayerId = p.Id,
                    Username = p.User?.Username ?? "Unknown",
                    ActiveBounties = p.ActiveBounties
                })
                .ToList();

            await _gameNotifier.KnockoutTargetsUpdated(game.Id, knockoutTargets);
        }

        public async Task RemovePlayerAsAdminAsync(int gameId, int playerId)
        {
            var game = await _context.Games
                .Include(g => g.Players)
                    .ThenInclude(p => p.User)
                .FirstOrDefaultAsync(g => g.Id == gameId);

            if (game == null)
                throw new KeyNotFoundException("Game not found");

            var player = game.Players.FirstOrDefault(p => p.Id == playerId);

            if (player == null)
                throw new Exception("Player not found");

            if (!player.IsActive)
                return;

            player.IsActive = false;
            player.LeftAt = DateTimeOffset.UtcNow;

            await _context.SaveChangesAsync();

            var knockoutTargets = game.Players
                .Where(p => p.IsActive)
                .Select(p => new KnockoutTargetDto
                {
                    PlayerId = p.Id,
                    Username = p.User?.Username ?? "Unknown",
                    ActiveBounties = p.ActiveBounties
                })
                .ToList();

            await _gameNotifier.KnockoutTargetsUpdated(game.Id, knockoutTargets);
        }


    }
}