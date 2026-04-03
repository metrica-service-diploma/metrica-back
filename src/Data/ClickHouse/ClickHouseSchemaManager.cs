using ClickHouse.Driver;

namespace metrica_back.src.Data.ClickHouse;

public interface IClickHouseSchemaManager
{
    Task<bool> DatabaseExistsAsync(ClickHouseClient client, string databaseName);
    Task<bool> TableExistsAsync(ClickHouseClient client, string databaseName, string tableName);
    Task CreateDatabaseIfNotExistsAsync(ClickHouseClient client, string databaseName);
    Task CreateTableIfNotExistsAsync(
        ClickHouseClient client,
        string databaseName,
        string tableName
    );
}

public class ClickHouseSchemaManager : IClickHouseSchemaManager
{
    public async Task<bool> DatabaseExistsAsync(ClickHouseClient client, string databaseName)
    {
        var result = await client.ExecuteScalarAsync($"EXISTS DATABASE {databaseName}");
        return Convert.ToBoolean(result);
    }

    public async Task<bool> TableExistsAsync(
        ClickHouseClient client,
        string databaseName,
        string tableName
    )
    {
        var result = await client.ExecuteScalarAsync($"EXISTS TABLE {databaseName}.{tableName}");
        return Convert.ToBoolean(result);
    }

    public async Task CreateDatabaseIfNotExistsAsync(ClickHouseClient client, string databaseName)
    {
        await client.ExecuteNonQueryAsync($"CREATE DATABASE IF NOT EXISTS {databaseName}");
    }

    public async Task CreateTableIfNotExistsAsync(
        ClickHouseClient client,
        string databaseName,
        string tableName
    )
    {
        await client.ExecuteNonQueryAsync(
            $@"
            CREATE TABLE IF NOT EXISTS {databaseName}.{tableName} (
                Id String,
                ClientId String,
                SessionId String,
                TrackingCode Int32,
                EventType Nullable(String),
                Timestamp DateTime,
                PageUrl Nullable(String),
                PageTitle Nullable(String),
                Referrer Nullable(String),
                UserAgent Nullable(String),
                ScreenWidth Int32,
                ScreenHeight Int32,
                BrowserLanguage Nullable(String)
            ) ENGINE = MergeTree()
            ORDER BY Timestamp"
        );
    }
}
