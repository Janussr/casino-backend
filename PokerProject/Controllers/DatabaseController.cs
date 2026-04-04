using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokerProject.Services.Database;

namespace PokerProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DatabaseController : ControllerBase
    {
        private readonly IDatabaseService _databaseService;

        public DatabaseController(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        [HttpPost("ping")]
        [Authorize(Roles = "Admin,Gamemaster")]
        public async Task<IActionResult> Ping()
        {
            try
            {
                await _databaseService.PingAsync();
                return Ok(new { message = "Database ping successful" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}