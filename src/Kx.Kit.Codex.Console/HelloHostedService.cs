using Kx.Toolx.Codex.Source.MySql;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Kx.Toolx.Codex.Console;

internal class HelloHostedService(ILogger<HelloHostedService> logger, MySqlDbContext dbContext)
    : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("StartAsync");
        var tables = dbContext.Tables.Take(5).ToList();

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("StopAsync");
        return Task.CompletedTask;
    }
}
