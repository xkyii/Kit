
using Ks.Core.Utilities.System.IO;
using Kx.Kit.Codex;
using Kx.Kit.Codex.Console;
using Kx.Kit.Codex.Console.Services;
using Kx.Kit.Codex.Generator;
using Kx.Kit.Codex.Source.MySql;
using Serilog.Events;
using Serilog;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;


Log.Logger = new LoggerConfiguration()
#if DEBUG
    .MinimumLevel.Debug()
#else
    .MinimumLevel.Information()
#endif
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Async(c => c.File(Path.Combine(Pathx.Profile("codex"), "Logs", "logs.txt")))
    .WriteTo.Async(c => c.Console())
    .CreateLogger();

Log.Information("Starting console host.");


try
{
    var host = Host.CreateDefaultBuilder(args)
        .ConfigureAppConfiguration((context, builder) =>
        {
            var env = context.HostingEnvironment;
            var contentRootPath = context.HostingEnvironment.ContentRootPath;

            Log.Information(" EnvironmentName: {env}", env.EnvironmentName);

            builder.SetBasePath(Pathx.Profile("codex"));
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
            services.AddCodex();
            services.AddCodexGenerator();
            services.AddCodexMySql();
            services.AddSingleton<Db2JsonService>();
            services.AddSingleton<GenerateService>();
        })
        .UseConsoleLifetime()
        .Build()
        ;

    // var s = host.Services.GetRequiredService<Db2JsonService>();
    var s = host.Services.GetRequiredService<GenerateService>();
    await s.StartAsync(CancellationToken.None);
    await s.StopAsync(CancellationToken.None);
}
catch (Exception ex)
{
    Log.Error(ex.ToString());
}
finally
{
    Log.Information("Done.");
    Log.CloseAndFlush();
}
