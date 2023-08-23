using Serilog.Events;
using Serilog;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Kx.Toolx.Clicky;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

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
			builder.AddJsonFile("appsettings.json", true, true);
			builder.AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", true, true);
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