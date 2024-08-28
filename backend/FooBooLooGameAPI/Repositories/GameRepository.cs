using FooBooLooGameAPI.Data;
using FooBooLooGameAPI.Entities;
using FooBooLooGameAPI.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FooBooLooGameAPI.Repositories;

public class GameRepository : IGameRepository
{
    private readonly GameDbContext _context;

    public GameRepository(GameDbContext context)
    {
        _context = context;
    }

    public async Task<Game> CreateGameAsync(Game game)
    {
        // Check if a game with the same name already exists
        var existingGame = await _context.Games
            .Include(g => g.RuleSet)
            .FirstOrDefaultAsync(g => g.Name == game.Name);

        if (existingGame != null)
        {
            return null;
        }

        _context.Games.Add(game);
        await _context.SaveChangesAsync();
        return game;
    }

    public async Task<IEnumerable<Game>> GetGamesAsync()
    {
        return await _context.Games.Include(g => g.RuleSet).ToListAsync();
    }

    public async Task<Game> GetGameByIdAsync(int gameId)
    {
        return await _context.Games.Include(g => g.RuleSet).FirstOrDefaultAsync(g => g.GameId == gameId);
    }
}
