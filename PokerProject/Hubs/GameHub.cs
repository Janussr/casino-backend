using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace PokerProject.Hubs;

[Authorize]
public class GameHub : Hub
{
    public async Task JoinGameGroup(int gameId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"Game-{gameId}");
    }

    public async Task LeaveGameGroup(int gameId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"Game-{gameId}");
    }

}