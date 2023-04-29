using System.Data.Common;

namespace Kx.Codex.Db.Internal;

internal static class DbReaderExtension
{
    public static T? GetValueOrDefault<T>(this DbDataReader reader, string name)
    {
        var idx = reader.GetOrdinal(name);
        return reader.IsDBNull(idx)
            ? default
            : reader.GetFieldValue<T>(idx);
    }
}
