using FooBooLooGameAPI.DTO.Game;
using FooBooLooGameAPI.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace FooBooLooGameAPI.Controllers;

[EnableCors("AllowSpecificOrigin")]
[Route("api/[controller]")]
[ApiController]
public class GamesController : ControllerBase
{
    private readonly IGameService _gameService;

    public GamesController(IGameService gameService)
    {
        _gameService = gameService;
    }

    [HttpGet]
    public async Task<IActionResult> GetGames()
    {
        var response = await _gameService.GetGamesAsync();
        return StatusCode(response.statusCode, response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetGame(int id)
    {
        var response = await _gameService.GetGameAsync(id);

        return StatusCode(response.statusCode, response);
    }

    [HttpPost]
    public async Task<IActionResult> CreateGame([FromForm] CreateGameDto gameDto)
    {
        var response = await _gameService.CreateGameAsync(gameDto);

        return StatusCode(response.statusCode, response);
    }
}
