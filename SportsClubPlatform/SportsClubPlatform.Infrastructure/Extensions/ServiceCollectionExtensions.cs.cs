using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SportsClubPlatform.Application.Abstractions;
using SportsClubPlatform.Infrastructure.Persistence;
using SportsClubPlatform.Infrastructure.Services;

namespace SportsClubPlatform.Infrastructure.Extensions
{
    /// <summary>
    /// Dependency injection registration for infrastructure services.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            string connectionString =
                configuration.GetConnectionString("DefaultConnection")
                ?? "Data Source=sportsclubplatform.db";

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite(connectionString));

            services.AddScoped<ITransferApplicationService, TransferApplicationService>();

            return services;
        }
    }
}
