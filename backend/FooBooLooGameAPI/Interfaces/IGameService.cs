using FooBooLooGameAPI.DTO;
using FooBooLooGameAPI.DTO.Game;

namespace FooBooLooGameAPI.Interfaces;

public interface IGameService
{
    Task<ResponseDto> CreateGameAsync(CreateGameDto gameDto);
    Task<ResponseDto> GetGameAsync(int id);
    Task<ResponseDto> GetGamesAsync();
}