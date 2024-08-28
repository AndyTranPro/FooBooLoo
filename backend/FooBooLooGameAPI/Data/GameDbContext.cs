using FooBooLooGameAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace FooBooLooGameAPI.Data;

public class GameDbContext(DbContextOptions<GameDbContext> options) : DbContext(options)
{
    public DbSet<Game> Games { get; set; } = null!;
    public DbSet<Session> Sessions { get; set; } = null!;
    public DbSet<SessionNumber> SessionNumbers { get; set; } = null!;
    public DbSet<GameRule> GameRules { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Session>()
            .HasIndex(s => new { s.GameId, s.PlayerName, s.StartTime })
            .IsUnique();
    }
}
