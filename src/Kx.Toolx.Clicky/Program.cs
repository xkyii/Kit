using Serilog.Events;
using Serilog;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Kx.Toolx.Clicky;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;


var folder = genSpecialFolder();
Console.WriteLine($"App Folder: {folder}");


Log.Logger = new LoggerConfiguration()
#if DEBUG
	.MinimumLevel.Debug()
#else
	.MinimumLevel.Information()
#endif
	.MinimumLevel.Override("Microsoft", LogEventLevel.Information)
	.Enrich.FromLogContext()
	.WriteTo.Console()
	.CreateLogger();

Log.Information("Starting console host.");


try
{
	await Host.CreateDefaultBuilder(args)
		.ConfigureAppConfiguration((context, builder) =>
		{
			var env = context.HostingEnvironment;
			var contentRootPath = context.HostingEnvironment.ContentRootPath;

			Log.Information(" EnvironmentName: {env}", env.EnvironmentName);

			builder.SetBasePath(folder);
			builder.AddJsonFile("appsettings.json", true);
			builder.AddJsonFile(Path.Combine(contentRootPath, "appsettings.json"), true);
			builder.AddJsonFile(Path.Combine(contentRootPath, $"appsettings.{env.EnvironmentName}.json"), true);
		})
		.ConfigureLogging(builder =>
		{
			builder.ClearProviders();
			builder.AddSerilog();
		})
		.ConfigureServices((hostContext, services) =>
		{
			services.AddHostedService<HostedService>();
			services.Configure<ClickyConfig>(hostContext.Configuration.GetSection(ClickyConfig.KEY));
		})
		.UseConsoleLifetime()
		.Build()
		.RunAsync()
		;

}
catch (Exception ex)
{
	Log.Error(ex.ToString());
}
finally
{
	Log.CloseAndFlush();
}



static string genSpecialFolder()
{
	// 获取当前用户的用户文件夹路径
	string path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
	string specificPath = Path.Combine(path, ".xky", "clicky");
	// 如果子文件夹不存在，则创建它
	if (!Directory.Exists(specificPath))
	{
		Directory.CreateDirectory(specificPath);
	}
	return specificPath;
}