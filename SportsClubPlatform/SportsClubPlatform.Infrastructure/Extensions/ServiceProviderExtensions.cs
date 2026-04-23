using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SportsClubPlatform.Infrastructure.Persistence;
using SportsClubPlatform.Infrastructure.Persistence.Seed;

namespace SportsClubPlatform.Infrastructure.Extensions
{
    /// <summary>
    /// Startup extensions for infrastructure concerns.
    /// </summary>
    public static class ServiceProviderExtensions
    {
        public static async Task InitializeDatabaseAsync(this IServiceProvider serviceProvider)
        {
            using IServiceScope scope = serviceProvider.CreateScope();

            AppDbContext dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            await dbContext.Database.EnsureCreatedAsync();
            await DbSeeder.SeedAsync(dbContext);
        }
    }
}
