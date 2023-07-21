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

	public static string CsType(this Column column)
	{
		return column.DataType switch
		{
			"tinyint" => "byte",
			"smallint" => "short",
			"int" => "int",
			"bigint" => "long",
			"decimal" => "decimal", // TODO
			"text" => "string",
			"json" => "string",
			"longtext" => "string",
			"varchar" => "string",
			"datetime" => "DateTime",
			"timestamp" => "DateTime",
			"time" => "DateTime",
			_ => "Unknown",
		};
	}
}
