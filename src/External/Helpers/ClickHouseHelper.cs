using ClickHouse.Driver.ADO.Parameters;
using ClickHouse.Driver.Utility;

namespace metrica_back.src.External.Helpers;

public static class ClickHouseHelper
{
    public static ClickHouseParameterCollection CreateParameters(
        params (string Name, object? Value)[] parameters
    )
    {
        var collection = new ClickHouseParameterCollection();

        foreach (var (name, value) in parameters)
        {
            collection.AddParameter(name, value);
        }

        return collection;
    }
}
