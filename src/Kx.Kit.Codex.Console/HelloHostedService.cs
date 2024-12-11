﻿using System.Text.Encodings.Web;
using System.Text.Json;
using Ks.Core.Utilities.System.IO;
using Kx.Kit.Codex.Source;
using Kx.Kit.Codex.Source.MySql;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Kx.Kit.Codex.Console;

internal class HelloHostedService(
    ILogger<HelloHostedService> logger,
    IConfiguration configuration,
    MySqlDbContext dbContext)
    : IHostedService
{
    private readonly string _generatedDirectory = configuration["Generate:GeneratedDirectory"] ?? "generated";
    private readonly string _tableFileName = configuration["Generate:TablesFileName"] ?? "Tables.json";

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

        File.WriteAllBytes(Path.Combine(_generatedDirectory, _tableFileName), JsonSerializer.SerializeToUtf8Bytes(tables, _jsonOptions));
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("StopAsync");
        return Task.CompletedTask;
    }

    private static void Insert(Table table, Column column)
    {
        if (table.Columns == null)
        {
            table.Columns = new List<Column>();
        }

        table.Columns.Add(column);
    }
}
