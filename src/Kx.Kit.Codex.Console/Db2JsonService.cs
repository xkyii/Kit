using System.Text.Encodings.Web;
using System.Text.Json;
using Ks.Core.Utilities.System.IO;
using Kx.Kit.Codex.Source;
using Kx.Kit.Codex.Source.MySql;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Kx.Kit.Codex.Console;

internal class Db2JsonService(
    ILogger<Db2JsonService> logger,
    IConfiguration configuration,
    MySqlDbContext dbContext)
{
    private readonly string _generatedDirectory = configuration["Generate:GeneratedDirectory"] ?? "generated";

    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        WriteIndented = true,
    };

    public Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("StartAsync");
        Pathx.EnsureDirectoryExists(Path.GetFullPath(_generatedDirectory));

        var tables = dbContext.Tables.ToList();
        var columns = dbContext.Columns.ToList();

        foreach (var table in tables)
        {
            foreach (var column in columns)
            {
                if (table.TableName == column.TableName)
                {
                    Insert(table, column);
                }
            }
        }

        foreach (var table in tables)
        {
            var json = JsonSerializer.SerializeToUtf8Bytes(table, _jsonOptions);
            File.WriteAllBytes(Path.Combine(_generatedDirectory, table.TableName + ".json"), json);
        }

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("StopAsync");
        return Task.CompletedTask;
    }

    private static void Insert(Table table, Column column)
    {
        table.Columns ??= new List<Column>();
        table.Columns.Add(column);
    }
}
