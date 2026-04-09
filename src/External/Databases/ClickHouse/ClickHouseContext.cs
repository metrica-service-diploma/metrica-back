using ClickHouse.Driver;
using metrica_back.src.External.Interfaces;
using Microsoft.Extensions.Options;

namespace metrica_back.src.External.Databases.ClickHouse;

public class ClickHouseContext(
    string connectionString,
    IClickHouseSchemaManager clickHouseSchemaManager,
    IOptions<ClickHouseOptions> options,
    ILogger<ClickHouseContext> logger
)
{
    public ClickHouseClient GetClient() => new(connectionString);

    public async Task InitializeAsync()
    {
        using var client = GetClient();

        await InitializeDatabaseAsync(client);
        await InitializeTableAsync(client);
    }

    private async Task InitializeDatabaseAsync(ClickHouseClient client)
    {
        if (await clickHouseSchemaManager.DatabaseExistsAsync(client))
            return;

        await clickHouseSchemaManager.CreateDatabaseIfNotExistsAsync(client);
        logger.LogInformation("Created database '{Database}'", options.Value.DatabaseName);
    }

    private async Task InitializeTableAsync(ClickHouseClient client)
    {
        if (await clickHouseSchemaManager.TableExistsAsync(client))
            return;

        await clickHouseSchemaManager.CreateTableIfNotExistsAsync(client);
        logger.LogInformation("Created table '{Table}'", options.Value.TableName);
    }
}
