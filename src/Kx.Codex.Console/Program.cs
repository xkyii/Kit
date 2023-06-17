using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;


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
	.WriteTo.Async(c => c.File(Path.Combine(folder, "Logs", "logs.txt")))
	.WriteTo.Async(c => c.Console())
	.CreateLogger();

Log.Information("Starting console host.");


try
{
	await Host.CreateDefaultBuilder(args)
		.ConfigureAppConfiguration((context, builder) =>
		{
			var env = context.HostingEnvironment;
			var contentRootPath = context.HostingEnvironment.ContentRootPath;

			builder.SetBasePath(folder);
			builder.AddJsonFile(Path.Combine(contentRootPath, "appsettings.json"), true);
			builder.AddJsonFile(Path.Combine(contentRootPath, $"appsettings.{env.EnvironmentName}.json"), true);
		})
		.ConfigureLogging(builder =>
		{
			builder.ClearProviders();
			builder.AddSerilog();
		})
		.UseConsoleLifetime()
		.Build()
		.RunAsync()
		;

}
catch (Exception ex)
{
	Console.WriteLine(ex.ToString());
}
finally
{
	Log.CloseAndFlush();
}







static string genSpecialFolder()
{
	// 获取当前用户的用户文件夹路径
	string path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
	string specificPath = Path.Combine(path, ".xky", "codex");
	// 如果子文件夹不存在，则创建它
	if (!Directory.Exists(specificPath))
	{
		Directory.CreateDirectory(specificPath);
	}
	return specificPath;
}