using Microsoft.EntityFrameworkCore;
using PokerProject.Data;
using PokerProject.DTOs;
using PokerProject.Models;

namespace PokerProject.Services.Participants
{
    public class ParticipantService : IParticipantService
    {
        private readonly PokerDbContext _context;

        public ParticipantService(PokerDbContext context)
        {
            _context = context;
        }


        public async Task AddParticipantsAsync(int gameId, List<int> userIds)
        {

            var game = await _context.Games.FindAsync(gameId);
            if (game == null)
                throw new KeyNotFoundException("Game not found");

            foreach (var userId in userIds)
            {
                var exists = await _context.GameParticipants
                    .AnyAsync(gp => gp.GameId == gameId && gp.UserId == userId);

                if (!exists)
                {
                    _context.GameParticipants.Add(new GameParticipant
                    {
                        GameId = gameId,
                        UserId = userId
                    });
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task<List<ParticipantDto>> GetParticipantsAsync(int gameId)
        {
            return await _context.GameParticipants
                .Where(gp => gp.GameId == gameId)
                .Include(gp => gp.User)
                .Select(gp => new ParticipantDto
                {
                    UserId = gp.UserId,
                    UserName = gp.User.Name
                })
                .ToListAsync();
        }

        public async Task<bool> IsUserParticipantAsync(int gameId, int userId)
        {
            return await _context.GameParticipants
                .AnyAsync(gp => gp.GameId == gameId && gp.UserId == userId);
        }

        public async Task<List<ParticipantDto>> RemoveParticipantAsync(int gameId, int userId)
        {
            var game = await _context.Games
                .Include(g => g.Participants)
                .FirstOrDefaultAsync(g => g.Id == gameId);

            if (game == null)
                throw new KeyNotFoundException("Game not found");

            if (game.IsFinished)
                throw new InvalidOperationException("Cannot remove participants from a finished game");

            var participant = await _context.GameParticipants
                .FirstOrDefaultAsync(p => p.GameId == gameId && p.UserId == userId);

            if (participant == null)
                throw new InvalidOperationException("User is not a participant in this game");

            _context.GameParticipants.Remove(participant);
            await _context.SaveChangesAsync();

            return await _context.GameParticipants
                .Where(p => p.GameId == gameId)
                .Include(p => p.User)
                .Select(p => new ParticipantDto
                {
                    UserId = p.UserId,
                    UserName = p.User.Name
                })
                .ToListAsync();
        }

    }
}
