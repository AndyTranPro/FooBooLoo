using FooBooLooGameAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace FooBooLooGameAPI.Extensions
{
    public static class MigrationExtensions
    {
        public static void ApplyMigrations(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<GameDbContext>();

            dbContext.Database.Migrate();
        }
    }
}
