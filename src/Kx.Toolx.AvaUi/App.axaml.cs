using System.Runtime.InteropServices;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Kx.Toolx.AvaUi.Views;

namespace Kx.Toolx.AvaUi;

public partial class App : Application
{

#if DEBUG
    [DllImport("Kernel32")]
    public static extern void AllocConsole();

    [DllImport("Kernel32", SetLastError = true)]
    public static extern void FreeConsole();
#endif

    public override void Initialize()
    {
#if DEBUG
        AllocConsole();
#endif
        AvaloniaXamlLoader.Load(this);
    }

#if DEBUG
    ~App()
    {
        FreeConsole();
    }
#endif

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow();
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new MainView();
        }

        base.OnFrameworkInitializationCompleted();
    }
}
