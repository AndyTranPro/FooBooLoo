using FooBooLooGameAPI.DTO.Game;
using FooBooLooGameAPI.Entities;

namespace FooBooLooGameAPI.Mappers;

public static class GameMappers
{
    public static Game ToCreateGameDto(this CreateGameDto gameDto)
    {
        return new Game
        {
            Name = gameDto.Name,
            Author = gameDto.Author,
            Min = gameDto.Min,
            Max = gameDto.Max,
            RuleSet = gameDto.RuleSet.Select(r => new GameRule
            {
                Divisor = r.Divisor,
                Replacement = r.Replacement
            }).ToList()
        };
    }
}