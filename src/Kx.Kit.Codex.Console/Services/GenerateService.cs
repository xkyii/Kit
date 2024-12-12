using System.Text.Json;
using Ks.Core.Utilities.System.Text.Json;
using Kx.Kit.Codex.Console.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Kx.Kit.Codex.Console.Services;

/// <summary>
/// 输入: dbjson文件,作为Model的一部分
/// 输入: 模板文件
/// 输出: 模板格式化后的文件
/// </summary>
/// <param name="logger"></param>
/// <param name="configuration"></param>
internal class GenerateService(
    ILogger<GenerateService> logger,
    IConfiguration configuration)
{
    private readonly string _sourceFile = configuration["Generate:SourceFile"] ?? string.Empty;
    private readonly string _sourceKey = configuration["Generate:SourceKey"] ?? "Table";

    private static readonly JsonSerializerOptions DynamicSerializerOptions = new()
    {
        Converters = { new DynamicJsonConverter() },
        WriteIndented = true
    };

    private static readonly JsonSerializerOptions ConfigurationSerializerOptions = new()
    {
        Converters = { new ConfigurationJsonConverter() },
        WriteIndented = true
    };

    public Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("StartAsync");

        if (!File.Exists(_sourceFile))
        {
            logger.LogError($"File {_sourceFile} NOT exists");
            return Task.CompletedTask;
        }

        var dynamicConfig = ConfigurationToDynamic(configuration);

        var jsonText = File.ReadAllText(_sourceFile);
        var serializerOptions = new JsonSerializerOptions
        {
            Converters = { new DynamicJsonConverter() },
            WriteIndented = true
        };
        dynamic model = new System.Dynamic.ExpandoObject();
        ((IDictionary<string, object>) model).Add(_sourceKey, JsonSerializer.Deserialize<dynamic>(jsonText, serializerOptions));
        model.Generate = dynamicConfig.Generate;

        logger.LogInformation($"{JsonSerializer.Serialize(model, new JsonSerializerOptions(){WriteIndented = true})}");
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("StopAsync");
        return Task.CompletedTask;
    }

    private static dynamic ConfigurationToDynamic(IConfiguration c)
    {
        var x = JsonSerializer.Serialize(c, ConfigurationSerializerOptions);
        var y = JsonSerializer.Deserialize<dynamic>(x, DynamicSerializerOptions);
        return y;
    }
}
