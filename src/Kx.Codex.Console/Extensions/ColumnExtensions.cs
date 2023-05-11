using Kx.Codex.Console.Db;

namespace Kx.Codex.Console.Extensions;

public static class ColumnExtensions
{
    public static string JavaType(this Column column)
    {
        return column.DataType switch
        {
            "tinyint" => "Byte",
            "smallint" => "Short",
            "int" => "Integer",
            "bigint" => "Long",
            "decimal" => "Decimal", // TODO
            "text" => "String",
            "json" => "String",
            "longtext" => "String",
            "varchar" => "String",
            "datetime" => "Date",
            "timestamp" => "Date",
            "time" => "Date",
            _ => "Unknown",
        };
    }
}
