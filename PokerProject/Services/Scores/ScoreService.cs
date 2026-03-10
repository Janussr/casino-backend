using Microsoft.EntityFrameworkCore;
using PokerProject.Data;
using PokerProject.DTOs;

namespace PokerProject.Services.Scores
{
    public class ScoreService : IScoreService
    {
        private readonly PokerDbContext _context;
        public ScoreService(PokerDbContext context)
        {
            _context = context;
        }

        public async Task<ScoreDto> AddScoreAsync(int gameId, int userId, int points)
        {
            var game = await _context.Games.FindAsync(gameId);
            if (game == null)
                throw new KeyNotFoundException("Game not found");

            if (game.IsFinished)
                throw new InvalidOperationException("Game has ended – Cant add points.");

            var score = new Score
            {
                GameId = gameId,
                UserId = userId,
                Points = points,
                CreatedAt = DateTime.UtcNow,
                Type = Score.ScoreType.Chips,
            };

            _context.Scores.Add(score);
            await _context.SaveChangesAsync();

            return new ScoreDto
            {
                UserId = score.UserId,
                Points = score.Points,
                GameId = score.GameId,
                Type = score.Type,
            };
        }

        public async Task<List<ScoreDto>> AddScoresBulkAsync(BulkAddScoresDto dto)
        {
            var game = await _context.Games
                .Include(g => g.Scores)
                .FirstOrDefaultAsync(g => g.Id == dto.GameId);

            if (game == null)
                throw new KeyNotFoundException("Game not found");

            if (game.IsFinished)
                throw new InvalidOperationException("Game has ended - cant add points.");

            var addedScores = new List<Score>();

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                foreach (var s in dto.Scores)
                {
                    var score = new Score
                    {
                        GameId = game.Id,
                        UserId = s.UserId,
                        Points = s.Points,
                        CreatedAt = DateTime.UtcNow,
                        Type = Score.ScoreType.Chips,
                    };
                    _context.Scores.Add(score);
                    addedScores.Add(score);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }

            return addedScores.Select(s => new ScoreDto
            {
                Id = s.Id,
                UserId = s.UserId,
                Points = s.Points,
                Type = s.Type
            }).ToList();
        }

        public async Task<PlayerScoreDetailsDto> GetPlayerScoreEntries(int gameId, int userId)
        {
            var scores = await _context.Scores.Include(s => s.VictimUser)
                .Where(s => s.GameId == gameId && s.UserId == userId)
                .OrderBy(s => s.CreatedAt)
                .Select(s => new ScoreEntryDto
                {
                    Id = s.Id,
                    Points = s.Points,
                    CreatedAt = s.CreatedAt,
                    Type = s.Type,
                    VictimUserId = s.VictimUserId,
                    VictimUserName = s.VictimUser != null ? s.VictimUser.Name : null
                })
                .ToListAsync();

            if (!scores.Any())
                throw new KeyNotFoundException("No scores found for this player in this game");

            var user = await _context.Users.FindAsync(userId);

            return new PlayerScoreDetailsDto
            {
                UserId = userId,
                UserName = user!.Name,
                TotalPoints = scores.Sum(s => s.Points),
                Entries = scores
            };
        }

        public async Task<ScoreDto> RemoveScoreAsync(int scoreId)
        {
            var score = await _context.Scores.FindAsync(scoreId);
            if (score == null)
                throw new KeyNotFoundException("Score not found");

            var game = await _context.Games.FindAsync(score.GameId);
            if (game == null)
                throw new KeyNotFoundException("Game not found");

            if (game.IsFinished)
                throw new InvalidOperationException("Game has ended - can't remove points.");

            score.Points = 0;
            await _context.SaveChangesAsync();

            return new ScoreDto
            {
                Id = score.Id,
                GameId = score.GameId,
                UserId = score.UserId,
                Points = score.Points
            };
        }

        public async Task<ScoreDto> RegisterRebuyAsync(int gameId, int actorUserId, int targetUserId, bool isAdmin)
        {
            var game = await _context.Games
                .Include(g => g.Participants)
                .FirstOrDefaultAsync(g => g.Id == gameId);

            if (game == null)
                throw new KeyNotFoundException("Game not found");

            if (game.RebuyValue == null)
                throw new InvalidOperationException("Rebuy value not set by admin");

            if (!isAdmin && actorUserId != targetUserId)
                throw new UnauthorizedAccessException("Players can only rebuy themselves");

            var participant = game.Participants.FirstOrDefault(p => p.UserId == targetUserId);

            if (participant == null)
                throw new InvalidOperationException("Target user is not participant in game");

            participant.RebuyCount++;

            var score = new Score
            {
                GameId = gameId,
                UserId = targetUserId,
                Points = -game.RebuyValue.Value,
                CreatedAt = DateTime.UtcNow,
                Type = Score.ScoreType.Rebuy,
            };

            _context.Scores.Add(score);
            await _context.SaveChangesAsync();

            return new ScoreDto
            {
                UserId = targetUserId,
                Points = score.Points,
                Type = score.Type
            };
        }


    }
    }
