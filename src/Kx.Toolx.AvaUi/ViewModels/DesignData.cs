using Microsoft.Extensions.Logging.Abstractions;

namespace Kx.Toolx.AvaUi.ViewModels;

internal static class DesignData
{

    public static MainViewModel MainViewModel = new MainViewModel(NullLogger<MainViewModel>.Instance)
    {
        Greeting = "Welcome to DesignMode"
    };
}
