using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using PokerProject.Data;
using PokerProject.Hubs.GameNotifier;
using PokerProject.Models;
using PokerProject.Services.Players;

namespace PokerProject.Tests.Services
{
    public class PlayerServiceTests
    {
        private PokerDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<PokerDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new PokerDbContext(options);
        }

        private PlayerService CreateService(PokerDbContext context)
        {
            var gameNotifierMock = new Mock<IGameNotifier>();
            return new PlayerService(context, gameNotifierMock.Object);
        }

        [Fact]
        public async Task IsUserAPlayerAsync_ShouldReturnTrue_WhenPlayerExists()
        {
            var context = GetDbContext();
            var service = CreateService(context);

            var game = new Game { GameNumber = 1 };
            context.Games.Add(game);
            await context.SaveChangesAsync();

            context.Players.Add(new Player
            {
                GameId = game.Id,
                UserId = 1
            });

            await context.SaveChangesAsync();

            var result = await service.IsUserAPlayerAsync(game.Id, 1);

            result.Should().BeTrue();
        }

        [Fact]
        public async Task IsUserAPlayerAsync_ShouldReturnFalse_WhenPlayerDoesNotExist()
        {
            var context = GetDbContext();
            var service = CreateService(context);

            var game = new Game { GameNumber = 1 };
            context.Games.Add(game);
            await context.SaveChangesAsync();

            var result = await service.IsUserAPlayerAsync(game.Id, 999);

            result.Should().BeFalse();
        }

        [Fact]
        public async Task LeaveGameAsPlayerAsync_ShouldSetPlayerInactive()
        {
            var context = GetDbContext();
            var service = CreateService(context);

            var user = new User
            {
                Username = "testuser",
                PasswordHash = "hash"
            };
            context.Users.Add(user);
            await context.SaveChangesAsync();

            var game = new Game { GameNumber = 1 };
            context.Games.Add(game);
            await context.SaveChangesAsync();

            var player = new Player
            {
                GameId = game.Id,
                UserId = user.Id,
                IsActive = true
            };
            context.Players.Add(player);

            await context.SaveChangesAsync();

            await service.LeaveGameAsPlayerAsync(game.Id, user.Id);

            var updatedPlayer = await context.Players.FirstAsync();
            updatedPlayer.IsActive.Should().BeFalse();
            updatedPlayer.LeftAt.Should().NotBeNull();
        }

        [Fact]
        public async Task LeaveGameAsPlayerAsync_ShouldThrow_WhenGameFinished()
        {
            var context = GetDbContext();
            var service = CreateService(context);

            var game = new Game
            {
                GameNumber = 1,
                IsFinished = true
            };
            context.Games.Add(game);

            await context.SaveChangesAsync();

            Func<Task> act = async () =>
                await service.LeaveGameAsPlayerAsync(game.Id, 1);

            await act.Should().ThrowAsync<Exception>();
        }

        [Fact]
        public async Task RemovePlayerAsAdminAsync_ShouldDeactivatePlayer()
        {
            var context = GetDbContext();
            var service = CreateService(context);

            var user = new User
            {
                Username = "testuser",
                PasswordHash = "hash"
            };
            context.Users.Add(user);
            await context.SaveChangesAsync();

            var game = new Game { GameNumber = 1 };
            context.Games.Add(game);
            await context.SaveChangesAsync();

            var player = new Player
            {
                GameId = game.Id,
                UserId = user.Id,
                IsActive = true
            };
            context.Players.Add(player);

            await context.SaveChangesAsync();

            await service.RemovePlayerAsAdminAsync(game.Id, player.Id);

            var updatedPlayer = await context.Players.FirstAsync();
            updatedPlayer.IsActive.Should().BeFalse();
            updatedPlayer.LeftAt.Should().NotBeNull();
        }

        [Fact]
        public async Task RemovePlayerAsAdminAsync_ShouldThrow_WhenPlayerNotFound()
        {
            var context = GetDbContext();
            var service = CreateService(context);

            var game = new Game { GameNumber = 1 };
            context.Games.Add(game);
            await context.SaveChangesAsync();

            Func<Task> act = async () =>
                await service.RemovePlayerAsAdminAsync(game.Id, 999);

            await act.Should().ThrowAsync<Exception>();
        }
    }
}