using System.Collections.Generic;

namespace Kx.Codex.Db;

public class Table
{
    public string TableName { get; set; }
    public string SchemaName { get; set; }
    public ICollection<Column> Columns { get; set; }
}
