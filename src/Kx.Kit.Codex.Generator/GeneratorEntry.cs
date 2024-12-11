using Microsoft.Extensions.DependencyInjection;

namespace Kx.Kit.Codex.Generator;

public static class GeneratorEntry
{
    public static void AddCodexGenerator(this IServiceCollection services)
    {
        services.AddTransient<RazorGenerator>();
    }
}
