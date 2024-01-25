using Avalonia.Logging;
using Microsoft.Extensions.Logging;

namespace Kx.Toolx.AvaUi.Desktop;

public class MicrosoftLogSink(ILoggerFactory loggerFactory, LogLevel minimumAvaloniaLogLevel) : ILogSink
{
    private readonly ILogger logger = loggerFactory.CreateLogger(nameof(Avalonia));

    public bool IsEnabled(LogEventLevel level, string area)
    {
        return true;
    }

    public void Log(LogEventLevel level, string area, object? source, string messageTemplate)
    {
        var logLevel = level.ToMicrosoftLogLevel();
        if (logLevel < minimumAvaloniaLogLevel)
        {
            return;
        }

        logger.Log(level.ToMicrosoftLogLevel(), messageTemplate);
    }

    public void Log(LogEventLevel level,
        string area,
        object? source,
        string messageTemplate,
        params object?[] propertyValues)
    {
        var logLevel = level.ToMicrosoftLogLevel();
        if (logLevel < minimumAvaloniaLogLevel)
        {
            return;
        }

        logger.Log(level.ToMicrosoftLogLevel(), messageTemplate, propertyValues);
    }
}
