using System;
using System.IO;
using System.Threading.Tasks;
using Castle.Core.Configuration;
using Kx.Codex.Db;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Volo.Abp;

namespace Kx.Codex;

public class Program
{
    public async static Task<int> Main(string[] args)
    {
        string folder = genSpecialFolder();
        Console.WriteLine("App Folder: " + folder);

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

        try
        {
            Log.Information("Starting console host.");

            var builder = Host.CreateDefaultBuilder(args);

            builder
                .ConfigureAppConfiguration((context, builder) =>
                {
                    var env = context.HostingEnvironment;
                    var contentRootPath = context.HostingEnvironment.ContentRootPath;

                    builder.SetBasePath(folder);
                    builder.AddJsonFile(Path.Combine(contentRootPath, "appsettings.json"), true);
                    builder.AddJsonFile(Path.Combine(contentRootPath, $"appsettings.{env.EnvironmentName}.json"), true);
                })
                .ConfigureServices(services =>
                {
                    services.AddCodexDb();
                    services.AddHostedService<CodexHostedService>();
                    services.AddApplicationAsync<CodexModule>(options =>
                    {
                        options.Services.ReplaceConfiguration(services.GetConfiguration());
                    });

                    //// 打印配置变量
                    //foreach (var item in services.GetConfiguration().AsEnumerable())
                    //{
                    //    Log.Information("{0}={1}", item.Key, item.Value);
                    //}
                })
                .ConfigureLogging(builder =>
                {
                    builder.ClearProviders();
                    builder.AddSerilog();
                })
                .AddAppSettingsSecretsJson()
                .UseAutofac()
                .UseConsoleLifetime();

            var host = builder.Build();
            await host.Services.GetRequiredService<IAbpApplicationWithExternalServiceProvider>().InitializeAsync(host.Services);

            await host.RunAsync();

            return 0;
        }
        catch (Exception ex)
        {
            if (ex is HostAbortedException)
            {
                throw;
            }

            Log.Fatal(ex, "Host terminated unexpectedly!");
            return 1;
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    private static string genSpecialFolder()
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
}
