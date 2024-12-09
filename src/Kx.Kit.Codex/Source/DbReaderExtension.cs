using System.Data.Common;

namespace Kx.Kit.Codex.Source;


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
