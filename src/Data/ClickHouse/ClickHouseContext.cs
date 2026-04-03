using ClickHouse.Driver;

namespace metrica_back.src.Data.ClickHouse;

public class ClickHouseContext(
    IConfiguration config,
    ILogger<ClickHouseContext> logger,
    IClickHouseSchemaManager clickHouseSchemaManager,
    IClickHouseDataSeeder clickHouseDataSeeder
)
{
    private readonly string databaseName = config["ClickHouse:DatabaseName"];
    private readonly string tableName = config["ClickHouse:TableName"];

    public ClickHouseClient GetClient() => new(config.GetConnectionString("ClickHouseConnection"));

    public async Task InitializeAsync()
    {
        using var client = GetClient();

        await InitializeDatabaseAsync(client);
        await InitializeTableAsync(client);
    }

    private async Task InitializeDatabaseAsync(ClickHouseClient client)
    {
        if (await clickHouseSchemaManager.DatabaseExistsAsync(client, databaseName))
            return;

        await clickHouseSchemaManager.CreateDatabaseIfNotExistsAsync(client, databaseName);
        logger.LogInformation("Created database '{Database}'", databaseName);
    }

    private async Task InitializeTableAsync(ClickHouseClient client)
    {
        if (await clickHouseSchemaManager.TableExistsAsync(client, databaseName, tableName))
            return;

        await clickHouseSchemaManager.CreateTableIfNotExistsAsync(client, databaseName, tableName);
        await clickHouseDataSeeder.SeedIfEmptyAsync(client, databaseName, tableName);

        logger.LogInformation(
            "Created table '{Table}' with seed data",
            $"{databaseName}.{tableName}"
        );
    }
}
