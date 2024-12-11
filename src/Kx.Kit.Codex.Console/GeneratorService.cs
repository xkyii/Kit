using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Kx.Kit.Codex.Console;

internal class GeneratorService(
    ILogger<GeneratorService> logger,
    IConfiguration configuration)
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("StartAsync");
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("StopAsync");
        return Task.CompletedTask;
    }
}
