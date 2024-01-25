using System;
using Avalonia.Logging;
using Microsoft.Extensions.Logging;

namespace Kx.Toolx.AvaUi.Desktop;

public static class LogLevelExtensions
{
    public static LogLevel ToMicrosoftLogLevel(this LogEventLevel level)
    {
        return level switch
        {
            LogEventLevel.Verbose => LogLevel.Trace,
            LogEventLevel.Debug => LogLevel.Debug,
            LogEventLevel.Information => LogLevel.Information,
            LogEventLevel.Warning => LogLevel.Warning,
            LogEventLevel.Error => LogLevel.Error,
            LogEventLevel.Fatal => LogLevel.Critical,
            _ => throw new ArgumentOutOfRangeException(nameof(level), level, null)
        };
    }
}
