using ClickHouse.Driver;

namespace metrica_back.src.External.Interfaces;

public interface IClickHouseSchemaManager
{
    Task<bool> DatabaseExistsAsync(ClickHouseClient client);
    Task<bool> TableExistsAsync(ClickHouseClient client);
    Task CreateDatabaseIfNotExistsAsync(ClickHouseClient client);
    Task CreateTableIfNotExistsAsync(ClickHouseClient client);
}
