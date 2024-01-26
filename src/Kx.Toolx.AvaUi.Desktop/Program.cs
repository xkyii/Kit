using System;
using Avalonia;
using Kx.Toolx.AvaUi.Desktop.Logger;
using Microsoft.Extensions.DependencyInjection;
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
                services.AddAvaUi();
                services.AddLogging(builder =>
                {
                    builder.AddSplat();
                    builder.AddAllocConsole();
                });
            });
}
