using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Ks.Core.Naming;
using Kx.Codex.Console.Db;
using Kx.Codex.Console.Extensions;
using Kx.Codex.Console.Text;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RazorEngineCore;

namespace Kx.Codex.Console.Service;

internal class HelloHostedService : IHostedService
{
	private ILogger Logger { get; set; }

	private readonly CodexDbContext _dbContext;
	private readonly Dictionary<string, Dictionary<string, string>> _models;
	private readonly CommonModel _common;

	public HelloHostedService(CodexDbContext dbContext, ILogger<HelloHostedService> logger, IConfiguration configuration) 
	{
		_dbContext = dbContext;
		Logger = logger;
		_models = configuration.GetSection("Models")?.Get<Dictionary<string, Dictionary<string, string>>>() ?? new Dictionary<string, Dictionary<string, string>>();
		_common = configuration.GetSection("Common")?.Get<CommonModel>() ?? CommonModel.Empty;
	}

	public async Task StartAsync(CancellationToken cancellationToken)
	{
		Logger.LogInformation("RunAsync");
		var assmblyRefers = new List<Assembly>()
			{
				Assembly.Load("netstandard"),
				Assembly.Load("System.Private.CoreLib"),
				Assembly.Load("System.Runtime"),
				Assembly.Load("System.Collections"),
				Assembly.Load("System.ComponentModel"),
				Assembly.Load("System.Linq"),
				Assembly.Load("System.Linq.Expressions"),
				Assembly.Load("Microsoft.Extensions.DependencyInjection"),
				Assembly.Load("Microsoft.Extensions.DependencyInjection.Abstractions"),

				typeof(Program).Assembly,
				typeof(StringConvertExtensions).Assembly,
			}
		.Select(x => MetadataReference.CreateFromFile(x.Location))
		.ToImmutableList();

		var engine = new RazorEngine();
		var template = engine.Compile<RazorEngineTemplateBase<EntityModel>>(CONTENT, builder =>
		{

			foreach (var ar in assmblyRefers) 
			{
				builder.AddMetadataReference(ar);
			}

			builder.AddAssemblyReference(typeof(Column));
			builder.AddAssemblyReference(typeof(StringExtensions));
			builder.AddAssemblyReference(typeof(ColumnExtensions));
		});

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

			string result = await template.RunAsync(instance => instance.Model = model);

			Logger.LogInformation($"File: {table.TableName}");
			Logger.LogInformation($"{count}: {result}");

			string? path = string.Format(_models["Entity"]["File"], table.TableName.ToBeanName());
			await Write(path, result);
		}
	}

	public Task StopAsync(CancellationToken cancellationToken)
	{
		return Task.CompletedTask;
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


	private static readonly string CONTENT = """
		@using Kx.Codex.Console.Extensions
		@using Ks.Core.Naming;

		package @Model.Models["Entity"]["Package"];
		import com.aj.frame.beans.AJFrameNamedBeanAbstract;

		import java.util.List;


		 /**
		  * @@title: @Model.Table.TableComment
		  * @@author: @Model.Common.Author
		  * @@version @Model.Common.Version
		  */
		public class @Model.Table.TableName extends AJFrameNamedBeanAbstract {
		@foreach (var column in Model.Columns)
		{
		@:
		@:  /**
		@:   * @column.ColumnComment
		@:   */
		@:  private @column.JavaType() @column.ColumnName.SplitAuto().ToCamel();
		}
		}
		""";
}
