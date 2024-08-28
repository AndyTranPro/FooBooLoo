using FooBooLooGameAPI.DTO;
using FooBooLooGameAPI.DTO.Game;
using FooBooLooGameAPI.Entities;
using FooBooLooGameAPI.Interfaces;
using FooBooLooGameAPI.Mappers;
using FooBooLooGameAPI.Services;
using Moq;
using Xunit;

namespace FooBooLooGameAPI.Tests.Services
{
    public class GameServiceTests
    {
        private readonly GameService _gameService;
        private readonly Mock<IGameRepository> _mockGameRepository;

        public GameServiceTests()
        {
            _mockGameRepository = new Mock<IGameRepository>();
            _gameService = new GameService(_mockGameRepository.Object);
        }

        [Fact]
        public async Task CreateGameAsync_ShouldReturnSuccessResponse_WhenGameIsCreated()
        {
            // Arrange
            var createGameDto = new CreateGameDto
            {
                Name = "Test Game",
                Author = "First Author",
                Min = 10,
                Max = 20,
                RuleSet = new List<GameRuleDto>
                {
                    new GameRuleDto { Divisor = 3, Replacement = "Fizz" },
                    new GameRuleDto { Divisor = 5, Replacement = "Buzz" }
                }
            };

            var game = createGameDto.ToCreateGameDto();
            _mockGameRepository.Setup(repo => repo.CreateGameAsync(It.IsAny<Game>())).ReturnsAsync(game);

            // Act
            var result = await _gameService.CreateGameAsync(createGameDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(201, result.statusCode);
            Assert.Equal("Create Game Successfully", result.message);
            Assert.Equal(game, result.data);
        }

        [Fact]
        public async Task CreateGameAsync_ShouldReturnErrorResponse_WhenGameAlreadyExists()
        {
            // Arrange
            var createGameDto = new CreateGameDto
            {
                Name = "Existing Game",
                RuleSet = new List<GameRuleDto>() // Initialize RuleSet to avoid null reference
            };
            _mockGameRepository.Setup(repo => repo.CreateGameAsync(It.IsAny<Game>())).ReturnsAsync((Game?)null);

            // Act
            var result = await _gameService.CreateGameAsync(createGameDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(409, result.statusCode);
            Assert.Equal("Game Is Already Exist", result.message);
        }

        [Fact]
        public async Task GetGameAsync_ShouldReturnSuccessResponse_WhenGameExists()
        {
            // Arrange
            var gameId = 1;
            var game = new Game { GameId = gameId, Name = "Test Game" };
            _mockGameRepository.Setup(repo => repo.GetGameByIdAsync(gameId)).ReturnsAsync(game);

            // Act
            var result = await _gameService.GetGameAsync(gameId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.statusCode);
            Assert.Equal("Get Game Successfully", result.message);
            Assert.Equal(game, result.data);
        }

        [Fact]
        public async Task GetGameAsync_ShouldReturnErrorResponse_WhenGameDoesNotExist()
        {
            // Arrange
            var gameId = 999;
            _mockGameRepository.Setup(repo => repo.GetGameByIdAsync(gameId)).ReturnsAsync((Game?)null);

            // Act
            var result = await _gameService.GetGameAsync(gameId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(500, result.statusCode);
            Assert.Equal("Error while fetching game", result.message);
        }

        [Fact]
        public async Task GetGamesAsync_ShouldReturnSuccessResponse_WithListOfGames()
        {
            // Arrange
            var games = new List<Game>
            {
                new Game { Name = "Game 1" },
                new Game { Name = "Game 2" }
            };
            _mockGameRepository.Setup(repo => repo.GetGamesAsync()).ReturnsAsync(games);

            // Act
            var result = await _gameService.GetGamesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.statusCode);
            Assert.Equal("Get All Games Successfully", result.message);
            Assert.Equal(games, result.data);
        }
    }
}