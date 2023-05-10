using System.Threading.Tasks;
using Ks.Core.Naming;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Volo.Abp;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;
using Volo.Abp.TextTemplating.Razor;
using Volo.Abp.VirtualFileSystem;

namespace Kx.Codex.Console;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(AbpTextTemplatingRazorModule)
)]
public class CodexModule : AbpModule
{
    public override Task OnApplicationInitializationAsync(ApplicationInitializationContext context)
    {
        var logger = context.ServiceProvider.GetRequiredService<ILogger<CodexModule>>();
        var configuration = context.ServiceProvider.GetRequiredService<IConfiguration>();
        logger.LogInformation($"MySettingName => {configuration["MySettingName"]}");

        var hostEnvironment = context.ServiceProvider.GetRequiredService<IHostEnvironment>();
        logger.LogInformation($"EnvironmentName => {hostEnvironment.EnvironmentName}");

        return Task.CompletedTask;
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<CodexModule>("Kx.Codex.Console");
        });

        Configure<AbpRazorTemplateCSharpCompilerOptions>(options =>
        {
            options.References.Add(MetadataReference.CreateFromFile(typeof(CodexModule).Assembly.Location));
            options.References.Add(MetadataReference.CreateFromFile(typeof(StringConvertExtensions).Assembly.Location));
        });
    }

}
