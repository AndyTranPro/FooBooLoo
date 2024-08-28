using FooBooLooGameAPI.DTO;
using FooBooLooGameAPI.DTO.Session;
using FooBooLooGameAPI.Entities;
using FooBooLooGameAPI.Interfaces;
using FooBooLooGameAPI.Services;
using FooBooLooGameAPI.Helpers;
using Moq;
using Xunit;
using FooBooLooGameAPI.Mappers;

namespace FooBooLooGameAPI.Tests.Services;
public class SessionServiceTests
{
    private readonly Mock<ISessionRepository> _sessionRepositoryMock;
    private readonly Mock<IGameRepository> _gameRepositoryMock;
    private readonly SessionService _sessionService;

    public SessionServiceTests()
    {
        _sessionRepositoryMock = new Mock<ISessionRepository>();
        _gameRepositoryMock = new Mock<IGameRepository>();
        _sessionService = new SessionService(_sessionRepositoryMock.Object, _gameRepositoryMock.Object);
    }

    [Fact]
    public async Task StartSessionAsync_ShouldThrowArgumentException_WhenGameIsNull()
    {
        // Arrange
        var startSessionDto = new StartSessionDto { GameId = 1 };

        _gameRepositoryMock.Setup(repo => repo.GetGameByIdAsync(startSessionDto.GameId)).ReturnsAsync((Game?)null);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _sessionService.StartSessionAsync(startSessionDto));
    }

    [Fact]
    public async Task StartSessionAsync_ShouldReturnErrorResponse_WhenSessionIsNull()
    {
        // Arrange
        var startSessionDto = new StartSessionDto { GameId = 1 };
        var game = new Game { GameId = 1, Name = "Test Game" };

        _gameRepositoryMock.Setup(repo => repo.GetGameByIdAsync(startSessionDto.GameId)).ReturnsAsync(game);
        _sessionRepositoryMock.Setup(repo => repo.CreateSessionAsync(It.IsAny<Session>())).ReturnsAsync((Session?)null);

        // Act
        var result = await _sessionService.StartSessionAsync(startSessionDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(400, result.statusCode); // Assuming 400 is the status code for error response
        Assert.Equal("Failed to create session", result.message);
    }

    [Fact]
    public async Task StartSessionAsync_ShouldReturnSuccessResponse_WhenSessionIsCreated()
    {
        // Arrange
        var startSessionDto = new StartSessionDto { GameId = 1 };
        var game = new Game { GameId = 1, Name = "Test Game" };
        var session = new Session { SessionId = 1, GameId = 1, PlayerName = "Test Player" };

        _gameRepositoryMock.Setup(repo => repo.GetGameByIdAsync(startSessionDto.GameId)).ReturnsAsync(game);
        _sessionRepositoryMock.Setup(repo => repo.CreateSessionAsync(It.IsAny<Session>())).ReturnsAsync(session);

        // Act
        var result = await _sessionService.StartSessionAsync(startSessionDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(201, result.statusCode);
        Assert.Equal("Session created successfully", result.message);
    }

    [Fact]
    public async Task StartSessionAsync_ShouldThrowException_WhenGameDoesNotExist()
    {
        // Arrange
        var startSessionDto = new StartSessionDto { GameId = 1 };

        _gameRepositoryMock.Setup(repo => repo.GetGameByIdAsync(startSessionDto.GameId)).ReturnsAsync((Game?)null);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _sessionService.StartSessionAsync(startSessionDto));
    }

    [Fact]
    public async Task GetSessionAsync_ShouldReturnSuccessResponse_WhenSessionExists()
    {
        // Arrange
        var sessionId = 1;
        var session = new Session { SessionId = 1, GameId = 1, PlayerName = "Test Player" };

        _sessionRepositoryMock.Setup(repo => repo.GetSessionByIdAsync(sessionId)).ReturnsAsync(session);

        // Act
        var result = await _sessionService.GetSessionAsync(sessionId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(200, result.statusCode);
        Assert.Equal("Session retrieved successfully", result.message);
    }

    [Fact]
    public async Task GetSessionAsync_ShouldReturnErrorResponse_WhenSessionDoesNotExist()
    {
        // Arrange
        var sessionId = 1;

        _sessionRepositoryMock.Setup(repo => repo.GetSessionByIdAsync(sessionId)).ReturnsAsync((Session?)null);

        // Act
        var result = await _sessionService.GetSessionAsync(sessionId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(404, result.statusCode);
        Assert.Equal("Invalid SessionId, the session does not exist.", result.message);
    }

    [Fact]
    public async Task SubmitAnswerAsync_ShouldReturnSuccessResponse_WhenAnswerIsSubmittedCorrectly()
    {
        // Arrange
        var sessionId = 1;
        var submitAnswerDto = new SubmitAnswerDto { Number = 3, Answer = "Fizz" };
        var session = new Session
        {
            SessionId = 1,
            GameId = 1,
            PlayerName = "Test Player",
            StartTime = DateTime.UtcNow,
            Duration = 30,
            Game = new Game { RuleSet = new List<GameRule> { new GameRule { Divisor = 3, Replacement = "Fizz" } } },
            SessionNumbers = new List<SessionNumber>
            {
                new SessionNumber { NumberServed = 3, IsPending = true }
            }
        };

        _sessionRepositoryMock.Setup(repo => repo.GetSessionByIdAsync(sessionId)).ReturnsAsync(session);
        _sessionRepositoryMock.Setup(repo => repo.IsNumberServedAsync(sessionId, submitAnswerDto.Number)).ReturnsAsync(false);
        _sessionRepositoryMock.Setup(repo => repo.SaveChangesAsync()).Returns(Task.CompletedTask);

        // Act
        var result = await _sessionService.SubmitAnswerAsync(submitAnswerDto, sessionId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(200, result.statusCode);
        Assert.Equal("Answer submitted successfully, serving another number", result.message);
    }

    [Fact]
    public async Task SubmitAnswerAsync_ShouldReturnErrorResponse_WhenSessionDoesNotExist()
    {
        // Arrange
        var sessionId = 1;
        var submitAnswerDto = new SubmitAnswerDto { Number = 3, Answer = "Fizz" };

        _sessionRepositoryMock.Setup(repo => repo.GetSessionByIdAsync(sessionId)).ReturnsAsync((Session?)null);

        // Act
        var result = await _sessionService.SubmitAnswerAsync(submitAnswerDto, sessionId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(404, result.statusCode);
        Assert.Equal("Invalid SessionId, the session does not exist.", result.message);
    }

    [Fact]
    public async Task SubmitAnswerAsync_ShouldReturnErrorResponse_WhenSessionTimeHasExpired()
    {
        // Arrange
        var sessionId = 1;
        var submitAnswerDto = new SubmitAnswerDto { Number = 3, Answer = "Fizz" };
        var session = new Session
        {
            SessionId = 1,
            GameId = 1,
            PlayerName = "Test Player",
            StartTime = DateTime.UtcNow.AddMinutes(-31),
            Duration = 30,
            Game = new Game { RuleSet = new List<GameRule> { new GameRule { Divisor = 3, Replacement = "Fizz" } } },
            SessionNumbers = new List<SessionNumber>()
        };

        _sessionRepositoryMock.Setup(repo => repo.GetSessionByIdAsync(sessionId)).ReturnsAsync(session);
        _sessionRepositoryMock.Setup(repo => repo.EndSessionAsync(sessionId)).ReturnsAsync(session);

        // Act
        var result = await _sessionService.SubmitAnswerAsync(submitAnswerDto, sessionId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(200, result.statusCode);
        Assert.Equal("Session time has expired", result.message);
    }

    [Fact]
    public async Task SubmitAnswerAsync_ShouldReturnErrorResponse_WhenNumberIsAlreadyGuessed()
    {
        // Arrange
        var sessionId = 1;
        var submitAnswerDto = new SubmitAnswerDto { Number = 3, Answer = "Fizz" };
        var session = new Session
        {
            SessionId = 1,
            GameId = 1,
            PlayerName = "Test Player",
            StartTime = DateTime.UtcNow,
            Duration = 30,
            Game = new Game { RuleSet = new List<GameRule> { new GameRule { Divisor = 3, Replacement = "Fizz" } } },
            SessionNumbers = new List<SessionNumber> { new SessionNumber { NumberServed = 3, IsCorrect = true } }
        };

        _sessionRepositoryMock.Setup(repo => repo.GetSessionByIdAsync(sessionId)).ReturnsAsync(session);
        _sessionRepositoryMock.Setup(repo => repo.IsNumberServedAsync(sessionId, submitAnswerDto.Number)).ReturnsAsync(true);

        // Act
        var result = await _sessionService.SubmitAnswerAsync(submitAnswerDto, sessionId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(400, result.statusCode);
        Assert.Equal("The number is already guessed", result.message);
    }

    [Fact]
    public async Task GetSessionResultsAsync_ShouldReturnSuccessResponse_WhenSessionResultsAreRetrieved()
    {
        // Arrange
        var sessionId = 1;
        var sessionResult = new SessionResultDto { SessionId = 1, FinalScore = 100 };

        _sessionRepositoryMock.Setup(repo => repo.GetSessionResultsAsync(sessionId)).ReturnsAsync(sessionResult);

        // Act
        var result = await _sessionService.GetSessionResultsAsync(sessionId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(200, result.statusCode);
        Assert.Equal("Session results retrieved successfully", result.message);
    }

    [Fact]
    public async Task GetSessionResultsAsync_ShouldReturnErrorResponse_WhenSessionDoesNotExistOrIsNotEnded()
    {
        // Arrange
        var sessionId = 1;

        _sessionRepositoryMock.Setup(repo => repo.GetSessionResultsAsync(sessionId)).ReturnsAsync((SessionResultDto?)null);

        // Act
        var result = await _sessionService.GetSessionResultsAsync(sessionId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(404, result.statusCode);
        Assert.Equal("Not found session or is not ended yet.", result.message);
    }
}
