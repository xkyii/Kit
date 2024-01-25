using System;
using System.Runtime.InteropServices;
using Avalonia;
using Avalonia.Logging;
using Avalonia.ReactiveUI;
using Kx.Toolx.AvaUi.ViewModels;
using Kx.Toolx.AvaUi.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ReactiveUI.Avalonia.Splat;
using Splat.Microsoft.Extensions.Logging;

namespace Kx.Toolx.AvaUi.Desktop;

class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args) => BuildAvaloniaApp()
        .StartWithClassicDesktopLifetime(args);

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace()
            .UseReactiveUIWithMicrosoftDependencyResolver(services =>
            {
                services.AddTransient<MainViewModel>();
                services.AddTransient<MainWindow>();
                services.AddLogging(builder =>
                {
#if DEBUG
                    AllocConsole();
#endif
                    builder.AddSplat();
                    builder.AddConsole();

                });
                services.AddSingleton<ILogSink, MicrosoftLogSink>(serviceProvider =>
                {
                    var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
                    return new MicrosoftLogSink(loggerFactory, minimumAvaloniaLogLevel: LogLevel.Warning);
                });
            }, serviceProvider =>
            {
                ArgumentNullException.ThrowIfNull(serviceProvider);

                Logger.Sink = serviceProvider.GetRequiredService<ILogSink>();
            });

#if DEBUG
    [DllImport("Kernel32")]
    public static extern void AllocConsole();
#endif
}
