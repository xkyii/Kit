using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Kx.Codex.Console.Db;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Kx.Codex.Console.Service;

internal class HelloHostedService : IHostedService
{
	private ILogger Logger { get; set; }

	private readonly CodexDbContext _dbContext;


	public HelloHostedService(CodexDbContext dbContext, ILogger<HelloHostedService> logger) 
	{
		_dbContext = dbContext;
		Logger = logger;
	}

	public async Task StartAsync(CancellationToken cancellationToken)
	{
		Logger.LogInformation("RunAsync");

		var tables = _dbContext.Tables.Take(5).ToList();

		int count = 0;
		foreach (var table in tables)
		{
			count++;
			var columns = _dbContext.Columns.Where(x => x.TableName == table.TableName).ToList();
			Logger.LogInformation($"File: {table.TableName}");
			Logger.LogInformation($"{count}: {columns.Count}");

		}
	}

	public Task StopAsync(CancellationToken cancellationToken)
	{
		return Task.CompletedTask;
	}
}
