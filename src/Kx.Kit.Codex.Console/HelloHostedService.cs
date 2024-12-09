using System.Text.Json;
using Ks.Core.Utilities.System.IO;
using Kx.Kit.Codex.Source.MySql;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Kx.Kit.Codex.Console;

internal class HelloHostedService(ILogger<HelloHostedService> logger, IConfiguration configuration, MySqlDbContext dbContext)
    : IHostedService
{
    private readonly string _generatedDirectory = configuration["Generate:GeneratedDirectory"] ?? "generated";
    private readonly string _tableFileName = configuration["Generate:TablesFileName"] ?? "Tables.json";

    public Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("StartAsync");
        Pathx.EnsureDirectoryExists(Path.GetFullPath(_generatedDirectory));
        var tables = dbContext.Tables.ToList();

        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        var json = JsonSerializer.Serialize(tables, options);
        File.WriteAllText(Path.Combine(_generatedDirectory, _tableFileName), json);

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("StopAsync");
        return Task.CompletedTask;
    }
}
