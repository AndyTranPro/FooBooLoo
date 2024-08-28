using FooBooLooGameAPI.Controllers;
using FooBooLooGameAPI.DTO;
using FooBooLooGameAPI.DTO.Game;
using FooBooLooGameAPI.Entities;
using FooBooLooGameAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace FooBooLooGameAPI.Tests
{
    public class GamesControllerTests
    {
        private readonly GamesController _controller;
        private readonly Mock<IGameService> _mockGameService;

        public GamesControllerTests()
        {
            _mockGameService = new Mock<IGameService>();
            _controller = new GamesController(_mockGameService.Object);
        }

        [Fact]
        public async Task GetGames_ReturnsOkResult_WithListOfGames()
        {
            // Arrange
            var responseDto = new ResponseDto("Success", new List<Game> { new Game(), new Game() }, 200, true);
            _mockGameService.Setup(service => service.GetGamesAsync()).ReturnsAsync(responseDto);

            // Act
            var result = await _controller.GetGames();

            // Assert
            var okResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(responseDto, okResult.Value);
        }

        [Fact]
        public async Task GetGame_ReturnsOkResult_WithGame()
        {
            // Arrange
            var gameId = 1;
            var responseDto = new ResponseDto("Success", new Game(), 200, true);
            _mockGameService.Setup(service => service.GetGameAsync(gameId)).ReturnsAsync(responseDto);

            // Act
            var result = await _controller.GetGame(gameId);

            // Assert
            var okResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(responseDto, okResult.Value);
        }

        [Fact]
        public async Task CreateGame_ReturnsCreatedResult_WhenGameIsCreated()
        {
            // Arrange
            var createGameDto = new CreateGameDto();
            var responseDto = new ResponseDto("Created successfully", new Game(), 201, true);
            _mockGameService.Setup(service => service.CreateGameAsync(createGameDto)).ReturnsAsync(responseDto);

            // Act
            var result = await _controller.CreateGame(createGameDto);

            // Assert
            var createdResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(201, createdResult.StatusCode);
            Assert.Equal(responseDto, createdResult.Value);
        }

        [Fact]
        public async Task CreateGame_ReturnsBadRequest_WhenGameCreationFails()
        {
            // Arrange
            var createGameDto = new CreateGameDto();
            var responseDto = new ResponseDto("Error creating game.", null, 400, false);
            _mockGameService.Setup(service => service.CreateGameAsync(createGameDto)).ReturnsAsync(responseDto);

            // Act
            var result = await _controller.CreateGame(createGameDto);

            // Assert
            var badRequestResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
            Assert.Equal(responseDto, badRequestResult.Value);
        }
    }
}
