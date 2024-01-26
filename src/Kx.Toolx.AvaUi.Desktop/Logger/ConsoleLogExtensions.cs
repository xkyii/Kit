using System.Runtime.InteropServices;
using Microsoft.Extensions.Logging;

namespace Kx.Toolx.AvaUi.Desktop.Logger;

public static class ConsoleLogExtensions
{
    [DllImport("Kernel32")]
    private static extern void AllocConsole();

    public static ILoggingBuilder AddAllocConsole(this ILoggingBuilder builder)
    {
        AllocConsole();
        return ConsoleLoggerExtensions.AddConsole(builder);
    }
}
