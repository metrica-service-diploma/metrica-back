using metrica_back.src.External.Databases.PostgreSql;
using metrica_back.src.External.Interfaces;

namespace metrica_back.src.External.HostedServices;

public class DatabaseInitHostedService(
    IServiceProvider serviceProvider,
    ILogger<DatabaseInitHostedService> logger
) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting database initialization...");

        using var scope = serviceProvider.CreateScope();
        var postgreSqlContext = scope.ServiceProvider.GetRequiredService<PostgreSqlContext>();
        var databaseSeeder = scope.ServiceProvider.GetRequiredService<IDatabaseSeeder>();

        try
        {
            await postgreSqlContext.Database.EnsureCreatedAsync(cancellationToken);
            await databaseSeeder.SeedAsync();

            logger.LogInformation("Database initialization completed successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while initializing the database.");
            throw;
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Database initialization service is stopping.");
        return Task.CompletedTask;
    }
}
