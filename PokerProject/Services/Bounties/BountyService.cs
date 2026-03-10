using Microsoft.EntityFrameworkCore;
using PokerProject.Data;
using PokerProject.DTOs;

namespace PokerProject.Services.Bounties
{
    public class BountyService : IBountyService
    {
        private readonly PokerDbContext _context;

        public BountyService(PokerDbContext context)
        {
            _context = context;
        }

        private async Task HandleKnockoutAsync(int gameId, int killerUserId, int victimUserId, bool isAdmin)
        {
            var game = await _context.Games
                .Include(g => g.Scores)
                .FirstOrDefaultAsync(g => g.Id == gameId);

            if (game == null)
                throw new KeyNotFoundException("Game not found");

            if (!game.BountyValue.HasValue)
                throw new InvalidOperationException("Bounty value not set for this game");

            var participants = await _context.GameParticipants
                .Where(p => p.GameId == gameId && (p.UserId == killerUserId || p.UserId == victimUserId))
                .ToListAsync();

            if (participants.Count != 2)
            {
                if (!participants.Any(p => p.UserId == killerUserId))
                    throw new InvalidOperationException("Killer not found in game");

                if (!participants.Any(p => p.UserId == victimUserId))
                    throw new InvalidOperationException("Victim not found in game");
            }

            var killer = participants.First(p => p.UserId == killerUserId);
            var victim = participants.First(p => p.UserId == victimUserId);

            if (killer.UserId == victim.UserId)
                throw new InvalidOperationException("Cannot knock yourself out");

            var bountyValue = game.BountyValue.Value;
            var points = victim.ActiveBounties > 0
                ? victim.ActiveBounties * bountyValue
                : 0;

            game.Scores.Add(new Score
            {
                GameId = game.Id,
                UserId = killerUserId,
                Points = points,
                Type = Score.ScoreType.Bounty,
                VictimUserId = victimUserId,
                CreatedAt = DateTime.UtcNow
            });

            victim.ActiveBounties = 0;
            killer.ActiveBounties += 1;

            await _context.SaveChangesAsync();
        }

        public async Task RegisterKnockoutAsync(int gameId, int killerUserId, int victimUserId, bool isAdmin)
        {
            await HandleKnockoutAsync(gameId, killerUserId, victimUserId, isAdmin);
        }


        public async Task<List<BountyLeaderboardDto>> GetBountyLeaderboardAsync()
        {
            var knockoutsQuery = _context.Scores
                .Where(s => s.Type == Score.ScoreType.Bounty)
                .GroupBy(s => s.UserId)
                .Select(g => new
                {
                    UserId = g.Key,
                    Knockouts = g.Count(),
                    TotalBountyPoints = g.Sum(s => s.Points)
                });

            var timesKnockedOutQuery = _context.Scores
                .Where(s => s.Type == Score.ScoreType.Bounty && s.VictimUserId.HasValue)
                .GroupBy(s => s.VictimUserId.Value)
                .Select(g => new
                {
                    VictimUserId = g.Key,
                    TimesKnockedOut = g.Count()
                });

            var knockouts = await knockoutsQuery.ToListAsync();
            var timesKnockedOut = await timesKnockedOutQuery.ToListAsync();

            var userIds = knockouts.Select(k => k.UserId)
                .Union(timesKnockedOut.Select(t => t.VictimUserId))
                .ToList();

            var users = await _context.Users
                .Where(u => userIds.Contains(u.Id))
                .ToListAsync();

            var leaderboard = users.Select(u => new BountyLeaderboardDto
            {
                UserId = u.Id,
                UserName = u.Username,
                Knockouts = knockouts.FirstOrDefault(k => k.UserId == u.Id)?.Knockouts ?? 0,
                TimesKnockedOut = timesKnockedOut.FirstOrDefault(t => t.VictimUserId == u.Id)?.TimesKnockedOut ?? 0,
                TotalBountyPoints = knockouts.FirstOrDefault(k => k.UserId == u.Id)?.TotalBountyPoints ?? 0
            })
            .OrderByDescending(x => x.Knockouts)
            .ToList();

            return leaderboard;
        }



    }
}
