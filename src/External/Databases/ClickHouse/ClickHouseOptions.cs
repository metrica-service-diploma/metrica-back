namespace metrica_back.src.External.Databases.ClickHouse;

public class ClickHouseOptions
{
    public const string SectionName = "ClickHouse";

    public string DatabaseName { get; set; }
    public string TableName { get; set; }

    public string GetFullTableName() => $"{DatabaseName}.{TableName}";
}
