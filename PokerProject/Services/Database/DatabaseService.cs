using Microsoft.EntityFrameworkCore;
using PokerProject.Data;

namespace PokerProject.Services.Database
{
    public class DatabaseService : IDatabaseService
    {
        private readonly PokerDbContext _context;

        public DatabaseService(PokerDbContext context)
        {
            _context = context;
        }

        public async Task PingAsync()
        {
            await _context.Database.ExecuteSqlRawAsync("SELECT 1");
        }
    }
}
