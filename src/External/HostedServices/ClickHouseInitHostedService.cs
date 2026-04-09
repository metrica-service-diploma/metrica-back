using metrica_back.src.External.Databases.ClickHouse;

namespace metrica_back.src.External.HostedServices;

public class ClickHouseInitHostedService(
    IServiceProvider serviceProvider,
    ILogger<ClickHouseInitHostedService> logger
) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting ClickHouse initialization...");

        using var scope = serviceProvider.CreateScope();
        var clickHouseContext = scope.ServiceProvider.GetRequiredService<ClickHouseContext>();
        var clickHouseSeeder = scope.ServiceProvider.GetRequiredService<ClickHouseSeeder>();

        try
        {
            await clickHouseContext.InitializeAsync();
            await clickHouseSeeder.SeedAsync();

            logger.LogInformation("ClickHouse initialization completed successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while initializing the ClickHouse database.");
            throw;
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("ClickHouse initialization service is stopping.");
        return Task.CompletedTask;
    }
}
