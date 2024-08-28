using FooBooLooGameAPI.Controllers;
using FooBooLooGameAPI.DTO;
using FooBooLooGameAPI.DTO.Session;
using FooBooLooGameAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace FooBooLooGameAPI.Tests.Controllers
{
    public class SessionsControllerTests
    {
        private readonly Mock<ISessionService> _sessionServiceMock;
        private readonly SessionsController _sessionsController;

        public SessionsControllerTests()
        {
            _sessionServiceMock = new Mock<ISessionService>();
            _sessionsController = new SessionsController(_sessionServiceMock.Object);
        }

        [Fact]
        public async Task StartSession_ShouldReturnStatusCode201_WhenSessionIsCreated()
        {
            // Arrange
            var startSessionDto = new StartSessionDto { GameId = 1 };
            var responseDto = new ResponseDto("Session created successfully", null, 201, true);

            _sessionServiceMock.Setup(service => service.StartSessionAsync(startSessionDto)).ReturnsAsync(responseDto);

            // Act
            var result = await _sessionsController.StartSession(startSessionDto) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(201, result.StatusCode);
            Assert.Equal(responseDto, result.Value);
        }

        [Fact]
        public async Task GetSession_ShouldReturnOk_WhenSessionExists()
        {
            // Arrange
            var sessionId = 1;
            var responseDto = new ResponseDto("Session retrieved successfully", null, 200, true);

            _sessionServiceMock.Setup(service => service.GetSessionAsync(sessionId)).ReturnsAsync(responseDto);

            // Act
            var result = await _sessionsController.GetSession(sessionId) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(responseDto, result.Value);
        }

        [Fact]
        public async Task SubmitAnswer_ShouldReturnStatusCode200_WhenAnswerIsSubmitted()
        {
            // Arrange
            var sessionId = 1;
            var submitAnswerDto = new SubmitAnswerDto { Number = 3, Answer = "Fizz" };
            var responseDto = new ResponseDto("Answer submitted successfully, serving another number", null, 200, true);

            _sessionServiceMock.Setup(service => service.SubmitAnswerAsync(submitAnswerDto, sessionId)).ReturnsAsync(responseDto);

            // Act
            var result = await _sessionsController.SubmitAnswer(sessionId, submitAnswerDto) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(responseDto, result.Value);
        }

        [Fact]
        public async Task GetSessionResults_ShouldReturnStatusCode200_WhenResultsExist()
        {
            // Arrange
            var sessionId = 1;
            var responseDto = new ResponseDto("Session results retrieved successfully", null, 200, true);

            _sessionServiceMock.Setup(service => service.GetSessionResultsAsync(sessionId)).ReturnsAsync(responseDto);

            // Act
            var result = await _sessionsController.GetSessionResults(sessionId) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(responseDto, result.Value);
        }
    }
}
