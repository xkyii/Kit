using Microsoft.Extensions.Logging.Abstractions;
// ReSharper disable UnusedMember.Global

namespace Kx.Toolx.AvaUi.ViewModels;

internal static class DesignData
{

    public static readonly MainViewModel MainViewModel = new(NullLogger<MainViewModel>.Instance)
    {
        Greeting = "Welcome to DesignMode"
    };
}
