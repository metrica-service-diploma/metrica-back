using metrica_back.src.External.Databases.PostgreSql;

namespace metrica_back.src.External.HostedServices;

public class PostgreSqlInitHostedService(
    IServiceProvider serviceProvider,
    ILogger<PostgreSqlInitHostedService> logger
) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting PostgreSQL initialization...");

        using var scope = serviceProvider.CreateScope();
        var postgreSqlContext = scope.ServiceProvider.GetRequiredService<PostgreSqlContext>();
        var postgreSqlSeeder = scope.ServiceProvider.GetRequiredService<PostgreSqlSeeder>();

        try
        {
            await postgreSqlContext.Database.EnsureCreatedAsync(cancellationToken);
            await postgreSqlSeeder.SeedAsync();

            logger.LogInformation("PostgreSQL initialization completed successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while initializing the PostgreSQL database.");
            throw;
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("PostgreSQL initialization service is stopping.");
        return Task.CompletedTask;
    }
}
