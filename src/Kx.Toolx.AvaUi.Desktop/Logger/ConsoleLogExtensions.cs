using System.Runtime.InteropServices;
using Microsoft.Extensions.Logging;

namespace Kx.Toolx.AvaUi.Desktop.Logger;

public static class ConsoleLogExtensions
{
#if DEBUG
    [DllImport("Kernel32")]
    private static extern void AllocConsole();
#endif

    public static ILoggingBuilder AddAllocConsole(this ILoggingBuilder builder)
    {
#if DEBUG
        AllocConsole();
#endif
        return builder.AddConsole();
    }
}
