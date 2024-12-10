using System.Text.Encodings.Web;
using System.Text.Json;
using Ks.Core.Utilities.System.IO;
using Kx.Kit.Codex.Source;
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
    private readonly string _columnFileName = configuration["Generate:ColumnsFileName"] ?? "Columns.json";

    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        WriteIndented = true,
    };

    public Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("StartAsync");
        Pathx.EnsureDirectoryExists(Path.GetFullPath(_generatedDirectory));

        // tables
        {
            var tables = dbContext.Tables.ToList();
            var json = JsonSerializer.SerializeToUtf8Bytes(tables, _jsonOptions);
            File.WriteAllBytes(Path.Combine(_generatedDirectory, _tableFileName), json);
        }

        // columns
        {
            var columns = dbContext.Columns.ToList();
            var json = JsonSerializer.SerializeToUtf8Bytes(columns, _jsonOptions);
            File.WriteAllBytes(Path.Combine(_generatedDirectory, _columnFileName), json);
        }

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("StopAsync");
        return Task.CompletedTask;
    }
}
