using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Kx.Codex.Console.Db;
using Kx.Codex.Console.Extensions;
using Kx.Codex.Console.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.TextTemplating;

namespace Kx.Codex.Console;

public class HelloWorldService : ITransientDependency
{
    public ILogger<HelloWorldService> Logger { get; set; }

    private readonly CodexDbContext _dbContext;
    private readonly ITemplateRenderer _templateRenderer;
    private readonly Dictionary<string, Dictionary<string, string>> _models;
    private readonly CommonModel _common;

	public HelloWorldService(CodexDbContext dbContext, ITemplateRenderer templateRenderer, IConfiguration configuration)
	{
		Logger = NullLogger<HelloWorldService>.Instance;
		_dbContext = dbContext;
        _templateRenderer = templateRenderer;
		_models = configuration.GetSection("Models")?.Get<Dictionary<string, Dictionary<string, string>>>() ?? new Dictionary<string, Dictionary<string, string>>();
        _common = configuration.GetSection("Common")?.Get<CommonModel>() ?? CommonModel.Empty;
    }

    public async Task SayHelloAsync()
    {
        Logger.LogInformation("Hello World!");

        var tables = _dbContext.Tables.Take(5).ToList();

        int count = 0;
        foreach (var table in tables)
        {
            count++;
            var columns = _dbContext.Columns.Where(x => x.TableName == table.TableName).ToList();
            var model = new EntityModel()
            {
                Table = table,
                Columns = columns,
                Models = _models,
                Common = _common,
            };
            var result = await _templateRenderer.RenderAsync("Entity", model);
            Logger.LogInformation($"File: {table.TableName}");
            Logger.LogInformation($"{count}: {result}");

            string? path = string.Format(_models["Entity"]["File"], table.TableName.ToBeanName());
            await Write(path, result);
        }
    }

    private async Task Write(string? path, string result)
    {
        Logger.LogInformation($"写入文件: {path}");
        if (path == null)
        {
            Logger.LogError("没有指定有效的文件.");
            return;
        }
        string? dir = Path.GetDirectoryName(path);
        if (dir == null)
        {
            Logger.LogError($"没有指定有效的目录. path: {path}");
            return;
        }
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }

        using (var file = new StreamWriter(path))
        {
            await file.WriteAsync(result);
        }
    }
}
