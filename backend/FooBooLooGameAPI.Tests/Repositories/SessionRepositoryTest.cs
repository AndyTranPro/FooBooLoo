using FooBooLooGameAPI.Data;
using FooBooLooGameAPI.Entities;
using FooBooLooGameAPI.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FooBooLooGameAPI.Tests.Repositories
{
    public class SessionRepositoryTests
    {
        private readonly GameDbContext _context;
        private readonly SessionRepository _sessionRepository;

        public SessionRepositoryTests()
        {
            // Get connection string
            var connectionString = "Server=localhost:5432;UserId=postgres;Password=postgres;Database=foobooloo-db";

            var options = new DbContextOptionsBuilder<GameDbContext>()
                .UseNpgsql(connectionString)
                .Options;

            _context = new GameDbContext(options);
            _sessionRepository = new SessionRepository(_context);

            // Ensure the database is created
            _context.Database.EnsureCreated();

            // Ensure the Games table is populated
            EnsureGamesTableIsPopulated().Wait();
        }

        private async Task EnsureGamesTableIsPopulated()
        {
            if (!_context.Games.Any())
            {
                var games = new List<Game>
                {
                    new Game { Name = "First Test Game" },
                    new Game { Name = "Second Test Game" },
                    new Game { Name = "Third Test Game" },
                    new Game { Name = "Fourth Test Game" }
                };

                _context.Games.AddRange(games);
                await _context.SaveChangesAsync();
            }
        }

        [Fact]
        public async Task CreateSessionAsync_ShouldReturnSession_WhenSessionIsCreated()
        {
            // Arrange
            var game = new Game
            {
                Name = "First Test Game",
            };
            _context.Games.Add(game);
            await _context.SaveChangesAsync();

            var session = new Session
            {
                PlayerName = "Test Player",
                GameId = game.GameId,
            };


            // Act
            var result = await _sessionRepository.CreateSessionAsync(session);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(session.PlayerName, result.PlayerName);
            Assert.Equal(session.GameId, result.GameId);
            Assert.True(result.StartTime <= DateTime.Now);
        }

        [Fact]
        public async Task GetSessionByIdAsync_ShouldReturnSession_WhenSessionExists()
        {
            // Arrange
            var game = new Game
            {
                Name = "Second Test Game",
            };
            _context.Games.Add(game);
            await _context.SaveChangesAsync();

            var session = new Session
            {
                PlayerName = "Test Player",
                GameId = game.GameId,
            };

            _context.Sessions.Add(session);
            await _context.SaveChangesAsync();

            // Act
            var result = await _sessionRepository.GetSessionByIdAsync(session.SessionId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(session.PlayerName, result.PlayerName);
            Assert.Equal(session.GameId, result.GameId);
        }

        [Fact]
        public async Task GetSessionByIdAsync_ShouldReturnNull_WhenSessionDoesNotExist()
        {
            // Act
            var result = await _sessionRepository.GetSessionByIdAsync(999);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task IsNumberServedAsync_ShouldReturnTrue_WhenNumberIsServed()
        {
            // Arrange
            var game = new Game
            {
                Name = "Third Test Game",
            };
            _context.Games.Add(game);
            await _context.SaveChangesAsync();

            var session = new Session
            {
                PlayerName = "Test Player",
                GameId = game.GameId,
            };
            _context.Sessions.Add(session);
            await _context.SaveChangesAsync();

            var sessionNumber = new SessionNumber
            {
                SessionId = session.SessionId,
                NumberServed = 5,
                IsCorrect = true
            };
            _context.SessionNumbers.Add(sessionNumber);
            await _context.SaveChangesAsync();

            // Act
            var result = await _sessionRepository.IsNumberServedAsync(session.SessionId, 5);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task IsNumberServedAsync_ShouldReturnFalse_WhenNumberIsNotServed()
        {
            // Arrange
            var game = new Game
            {
                Name = "Fourth Test Game",
            };
            _context.Games.Add(game);
            await _context.SaveChangesAsync();

            var session = new Session
            {
                PlayerName = "Test Player",
                GameId = game.GameId,
                SessionNumbers =
                [
                    new SessionNumber
                    {
                        NumberServed = 5,
                        IsCorrect = true
                    }
                ]
            };

            _context.Sessions.Add(session);
            await _context.SaveChangesAsync();

            int mockNumber = 6;

            // Act
            var result = await _sessionRepository.IsNumberServedAsync(session.SessionId, mockNumber);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task EndSessionAsync_ShouldReturnSession_WhenSessionIsEnded()
        {
            // Arrange
            var game = new Game
            {
                Name = "Test Game for EndSession"
            };
            _context.Games.Add(game);
            await _context.SaveChangesAsync();

            var session = new Session
            {
                PlayerName = "Test Player",
                GameId = game.GameId,
                StartTime = DateTime.UtcNow,
                IsEnded = false
            };
            _context.Sessions.Add(session);
            await _context.SaveChangesAsync();

            // Act
            var result = await _sessionRepository.EndSessionAsync(session.SessionId);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsEnded);
        }

        [Fact]
        public async Task EndSessionAsync_ShouldReturnNull_WhenSessionDoesNotExist()
        {
            // Act
            var result = await _sessionRepository.EndSessionAsync(999);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetSessionResultsAsync_ShouldReturnSessionResultDto_WhenSessionIsEnded()
        {
            // Arrange
            var game = new Game
            {
                Name = "Test Game for GetSessionResults"
            };
            _context.Games.Add(game);
            await _context.SaveChangesAsync();

            var session = new Session
            {
                PlayerName = "Test Player",
                GameId = game.GameId,
                StartTime = DateTime.UtcNow,
                IsEnded = true,
                Score = 100
            };
            _context.Sessions.Add(session);
            await _context.SaveChangesAsync();

            var sessionNumbers = new List<SessionNumber>
            {
                new SessionNumber { SessionId = session.SessionId, NumberServed = 1, IsCorrect = true },
                new SessionNumber { SessionId = session.SessionId, NumberServed = 2, IsCorrect = false }
            };
            _context.SessionNumbers.AddRange(sessionNumbers);
            await _context.SaveChangesAsync();

            // Act
            var result = await _sessionRepository.GetSessionResultsAsync(session.SessionId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(session.SessionId, result.SessionId);
            Assert.Equal(session.PlayerName, result.PlayerName);
            Assert.Equal(2, result.TotalQuestions);
            Assert.Equal(1, result.CorrectAnswers);
            Assert.Equal(1, result.IncorrectAnswers);
            Assert.Equal(session.Score, result.FinalScore);
        }

        [Fact]
        public async Task GetSessionResultsAsync_ShouldReturnSessionResultDto_SessionIsEnded()
        {
            // Arrange
            var game = new Game
            {
                Name = "Test Game for GetSessionResultsNotEnded"
            };
            _context.Games.Add(game);
            await _context.SaveChangesAsync();

            var session = new Session
            {
                PlayerName = "Test Player",
                GameId = game.GameId,
                StartTime = DateTime.UtcNow,
                IsEnded = false
            };
            _context.Sessions.Add(session);
            await _context.SaveChangesAsync();

            // Act
            var result = await _sessionRepository.GetSessionResultsAsync(session.SessionId);
            Assert.NotNull(result);
            var sessionFromDb = await _sessionRepository.GetSessionByIdAsync(session.SessionId);
            Assert.NotNull(sessionFromDb);
            Assert.True(sessionFromDb.IsEnded);
        }
    }
}
