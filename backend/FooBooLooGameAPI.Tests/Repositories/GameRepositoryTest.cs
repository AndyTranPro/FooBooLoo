using FooBooLooGameAPI.Data;
using FooBooLooGameAPI.DTO.Game;
using FooBooLooGameAPI.Entities;
using FooBooLooGameAPI.Mappers;
using FooBooLooGameAPI.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FooBooLooGameAPI.Tests.Repositories
{
    public class GameRepositoryTests
    {
        private readonly GameDbContext _context;
        private readonly GameRepository _gameRepository;

        public GameRepositoryTests()
        {
            // Get connection string
            var connectionString = "Server=localhost:5432;UserId=postgres;Password=postgres;Database=foobooloo-db";

            var options = new DbContextOptionsBuilder<GameDbContext>()
                .UseNpgsql(connectionString)
                .Options;

            _context = new GameDbContext(options);
            _gameRepository = new GameRepository(_context);

            // Ensure the database is created
            _context.Database.EnsureCreated();
        }

        [Fact]
        public async Task GetGamesAsync_ShouldReturnAllGames()
        {
            // Arrange
            _context.Games.RemoveRange(_context.Games);
            await _context.SaveChangesAsync();

            var games = new List<Game>
            {
                new Game
                {
                    Name = "Example Game 1",
                    Author = "Author 1",
                    Min = 1,
                    Max = 100,
                    RuleSet = new List<GameRule>
                    {
                        new GameRule
                        {
                            Divisor = 3,
                            Replacement = "Fizz"
                        },
                        new GameRule
                        {
                            Divisor = 5,
                            Replacement = "Buzz"
                        }
                    },
                },
                new Game
                {
                    Name = "Example Game 2",
                    Author = "Author 2",
                    Min = 5,
                    Max = 50,
                    RuleSet = new List<GameRule>
                    {
                        new GameRule
                        {
                            Divisor = 2,
                            Replacement = "Foo"
                        },
                        new GameRule
                        {
                            Divisor = 7,
                            Replacement = "Bar"
                        }
                    },
                }
            };

            _context.Games.AddRange(games);
            await _context.SaveChangesAsync();

            // Act
            var result = await _gameRepository.GetGamesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Contains(result, g => g.Name == "Example Game 1");
            Assert.Contains(result, g => g.Name == "Example Game 2");
        }

        [Fact]
        public async Task CreateGameAsync_ShouldReturnGame_WhenGameIsCreated()
        {
            // Arrange
            var createGameDto = new CreateGameDto
            {
                Name = "Test Game",
                Author = "First Author",
                Min = 10,
                Max = 20,
                RuleSet =
                [
                    new GameRuleDto()
                    {
                        Divisor = 3,
                        Replacement = "Fizz"
                    },

                    new GameRuleDto
                    {
                        Divisor = 5,
                        Replacement = "Buzz"
                    }
                ],
            };

            var game = createGameDto.ToCreateGameDto();

            // Act
            var result = await _gameRepository.CreateGameAsync(game);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(game.Name, result.Name);
            Assert.Equal(game.Author, result.Author);
            Assert.Equal(game.Min, result.Min);
            Assert.Equal(game.Max, result.Max);
            Assert.True(game.CreatedAt <= DateTime.Now);
        }

        [Fact]
        public async Task CreateGameAsync_ShouldReturnNull_WhenGameAlreadyExists()
        {
            // Arrange
            var game = new Game { Name = "Existing Game" };
            _context.Games.Add(game);
            await _context.SaveChangesAsync();

            // Act
            var result = await _gameRepository.CreateGameAsync(new Game { Name = "Existing Game" });

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task AddGameRule_ShouldAddGameRuleToDatabase()
        {
            // Arrange
            var game = new Game
            {
                Name = "Test Game 1",
                Author = "Test Author",
                Min = 1,
                Max = 100
            };
            _context.Games.Add(game);
            await _context.SaveChangesAsync();

            var gameRule = new GameRule
            {
                GameId = game.GameId,
                Divisor = 3,
                Replacement = "Fizz"
            };

            // Act
            _context.GameRules.Add(gameRule);
            await _context.SaveChangesAsync();

            // Assert
            var retrievedGameRule = await _context.GameRules.FindAsync(gameRule.Id);
            Assert.NotNull(retrievedGameRule);
            Assert.Equal(gameRule.GameId, retrievedGameRule.GameId);
            Assert.Equal(gameRule.Divisor, retrievedGameRule.Divisor);
            Assert.Equal(gameRule.Replacement, retrievedGameRule.Replacement);
        }

        [Fact]
        public async Task GetGameByIdAsync_ShouldReturnGame_WhenGameExists()
        {
            // Arrange
            var game = new Game { Name = "Game 1" };
            _context.Games.Add(game);
            await _context.SaveChangesAsync();

            // Act
            var result = await _gameRepository.GetGameByIdAsync(game.GameId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(game.Name, result.Name);
        }

        [Fact]
        public async Task GetGameByIdAsync_ShouldReturnNull_WhenGameDoesNotExist()
        {
            // Act
            var result = await _gameRepository.GetGameByIdAsync(999);

            // Assert
            Assert.Null(result);
        }
    }
}
