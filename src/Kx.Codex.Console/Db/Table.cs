using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kx.Codex.Db;

[Table("TABLES", Schema = "INFORMATION_SCHEMA")]
public class Table
{
    [Column("TABLE_CATALOG")]
    public string TableCatalog { get; private set; }

    [Column("TABLE_SCHEMA")]
    public string TableSchema { get; private set; }

    [Column("TABLE_NAME")]
    public string TableName { get; private set; }

    [Column("TABLE_TYPE")]
    public string TableType { get; private set; }

    [Column("ENGINE")]
    public string Engine { get; private set; }

    [Column("VERSION")]
    public ulong? Version { get; private set; }

    [Column("ROW_FORMAT")]
    public string RowFormat { get; private set; }

    [Column("TABLE_ROWS")]
    public ulong? TableRows { get; private set; }

    [Column("AVG_ROW_LENGTH")]
    public ulong? AvgRowLength { get; private set; }

    [Column("DATA_LENGTH")]
    public ulong? DataLength { get; private set; }

    [Column("MAX_DATA_LENGTH")]
    public ulong? MaxDataLength { get; private set; }

    [Column("INDEX_LENGTH")]
    public ulong? IndexLength { get; private set; }

    [Column("DATA_FREE")]
    public ulong? DataFree { get; private set; }

    [Column("AUTO_INCREMENT")]
    public ulong? AutoIncrement { get; private set; }

    [Column("CREATE_TIME")]
    public DateTime? CreateTime { get; private set; }

    [Column("UPDATE_TIME")]
    public DateTime? UpdateTime { get; private set; }

    [Column("CHECK_TIME")]
    public DateTime? CheckTime { get; private set; }

    [Column("TABLE_COLLATION")]
    public string TableCollation { get; private set; }

    [Column("CHECKSUM")]
    public ulong? Checksum { get; private set; }

    [Column("CREATE_OPTIONS")]
    public string CreateOptions { get; private set; }

    [Column("TABLE_COMMENT")]
    public string TableComment { get; private set; }
}
