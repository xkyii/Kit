using Serilog.Events;
using Serilog;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Kx.Toolx.Clicky;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

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

                // Options
                // 原配置
                // services.Configure<ClickyConfig>(hostContext.Configuration.GetSection(ClickyConfig.KEY));

                // 替换后
                services.AddOptions();
                var config = hostContext.Configuration.GetSection(ClickyConfig.KEY);
                services.AddSingleton<IOptionsChangeTokenSource<ClickyConfig>>(
                    new ConfigurationChangeTokenSource<ClickyConfig>(string.Empty, config));
                services.AddSingleton<IConfigureOptions<ClickyConfig>>(
                    new ClickyConfigureNamedOptions(string.Empty, config));
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

return;


static string genSpecialFolder()
{
    // 获取当前用户的用户文件夹路径
    var path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
    var specificPath = Path.Combine(path, ".xky", "clicky");
    // 如果子文件夹不存在，则创建它
    if (!Directory.Exists(specificPath))
    {
        Directory.CreateDirectory(specificPath);
    }

    return specificPath;
}
