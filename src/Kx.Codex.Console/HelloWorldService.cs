using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kx.Codex.Console.Db;
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
    private readonly Dictionary<string, Dictionary<string, string>> _configs = default;

	public HelloWorldService(CodexDbContext dbContext, ITemplateRenderer templateRenderer, IConfiguration configuration)
	{
		Logger = NullLogger<HelloWorldService>.Instance;
		_dbContext = dbContext;
        _templateRenderer = templateRenderer;
		_configs = configuration.GetSection("Models").Get<Dictionary<string, Dictionary<string, string>>>()!;
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
                Configs = _configs,
            };
            var result = await _templateRenderer.RenderAsync("Entity", model);
            Logger.LogInformation($"{count}: {result}");
        }

    }
}
