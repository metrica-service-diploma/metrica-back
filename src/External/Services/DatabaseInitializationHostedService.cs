using metrica_back.src.Core.Interfaces.Services;
using metrica_back.src.External.Databases.PostgreSql;

namespace metrica_back.src.External.Services;

public class DatabaseInitializationHostedService(
    IServiceProvider serviceProvider,
    ILogger<DatabaseInitializationHostedService> logger
) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting database initialization...");

        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<PostgreSqlContext>();
        var seeder = scope.ServiceProvider.GetRequiredService<IDatabaseSeeder>();

        try
        {
            await dbContext.Database.EnsureCreatedAsync(cancellationToken);
            await seeder.SeedAsync();

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
