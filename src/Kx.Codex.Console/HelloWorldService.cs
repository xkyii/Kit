using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kx.Codex.Console.Db;
using Kx.Codex.Console.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.TextTemplating;

namespace Kx.Codex.Console;

public class HelloWorldService : ITransientDependency
{
    public ILogger<HelloWorldService> Logger { get; set; }

    private readonly CodexDbContext _dbContext;
    private readonly ITemplateRenderer _templateRenderer;
    private readonly IOptions<List<ModelConfig>> _modelsConfigs;

	public HelloWorldService(CodexDbContext dbContext, ITemplateRenderer templateRenderer, IOptions<List<ModelConfig>> modelsConfigs)
	{
		Logger = NullLogger<HelloWorldService>.Instance;
		_dbContext = dbContext;
		_templateRenderer = templateRenderer;
		_modelsConfigs = modelsConfigs;
	}

	public async Task SayHelloAsync()
    {
        Logger.LogInformation("Hello World!");

        var tables = _dbContext.Tables.ToList();

        int count = 0;
        foreach (var table in tables)
        {
            count++;
            var columns = _dbContext.Columns.Where(x => x.TableName == table.TableName).ToList();
            var model = new EntityModel()
            {
                Table = table,
                Columns = columns,
            };
            var result = await _templateRenderer.RenderAsync("Entity", model);
            Logger.LogInformation($"{count}: {result}");
        }

    }
}
