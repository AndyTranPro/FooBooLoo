using FooBooLooGameAPI.DTO;
using FooBooLooGameAPI.DTO.Game;
using FooBooLooGameAPI.Helpers;
using FooBooLooGameAPI.Interfaces;
using FooBooLooGameAPI.Mappers;

namespace FooBooLooGameAPI.Services;

public class GameService : IGameService
{
    private readonly IGameRepository _gameRepository;

    public GameService(IGameRepository gameRepository)
    {
        _gameRepository = gameRepository;
    }

    public async Task<ResponseDto> CreateGameAsync(CreateGameDto gameDto)
    {
        var createGame = gameDto.ToCreateGameDto();
        var returnGameResponse = await _gameRepository.CreateGameAsync(createGame);
        if (returnGameResponse == null) return ResponseHandler.errorResponse("Game Is Already Exist", 409);
        return ResponseHandler.successResponse(returnGameResponse, "Create Game Successfully", 201);
    }

    public async Task<ResponseDto> GetGameAsync(int id)
    {
        var game = await _gameRepository.GetGameByIdAsync(id);
        if (game == null) return ResponseHandler.errorResponse("Error while fetching game", 500);

        return ResponseHandler.successResponse(game, "Get Game Successfully", 200);
    }

    public async Task<ResponseDto> GetGamesAsync()
    {
        var games = await _gameRepository.GetGamesAsync();
        return ResponseHandler.successResponse(games, "Get All Games Successfully", 200);
    }
}
