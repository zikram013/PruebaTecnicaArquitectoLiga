using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SportsClubPlatform.Application.Abstractions;
using SportsClubPlatform.Infrastructure.Messaging.Consumers;
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

            services.AddMassTransit(busConfigurator =>
            {
                busConfigurator.SetKebabCaseEndpointNameFormatter();

                busConfigurator.AddConsumer<TransferOfferSubmittedConsumer>();
                busConfigurator.AddConsumer<ValidateTransferBudgetConsumer>();
                busConfigurator.AddConsumer<TransferBudgetValidatedConsumer>();
                busConfigurator.AddConsumer<ValidatePlayerContractConsumer>();
                busConfigurator.AddConsumer<PlayerContractValidatedConsumer>();
                busConfigurator.AddConsumer<ProcessTransferPaymentConsumer>();
                busConfigurator.AddConsumer<TransferPaymentProcessedConsumer>();
                busConfigurator.AddConsumer<UpdateTransferSquadsConsumer>();
                busConfigurator.AddConsumer<TransferSquadsUpdatedConsumer>();
                busConfigurator.AddConsumer<GenerateTransferContractConsumer>();
                busConfigurator.AddConsumer<TransferContractGeneratedConsumer>();
                busConfigurator.AddConsumer<NotifyTransferPartiesConsumer>();
                busConfigurator.AddConsumer<TransferPartiesNotifiedConsumer>();

                busConfigurator.AddConsumer<TransferFailureConsumer>();
                busConfigurator.AddConsumer<CompensateTransferConsumer>();

                busConfigurator.UsingInMemory((context, cfg) =>
                {
                    cfg.ConfigureEndpoints(context);
                });
            });

            return services;
        }
    }
}
