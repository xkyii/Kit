using Microsoft.Extensions.DependencyInjection;

namespace Kx.Codex.Console.Db;

public static class CodexDbEntry
{
    public static void AddCodexDb(this IServiceCollection services)
    {
        services.AddDbContextFactory<CodexDbContext, CodexDbContextFactory>();
    }
}
