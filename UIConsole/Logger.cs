using Serilog;
namespace UIConsole;

public static class Logger
{
    private static ILogger _logger;

    public static void InitLogger()
    {
        _logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File("logs/console_log.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();
    }

    public static void LogInfo(string message)
    {
        _logger?.Information(message);
    }

    public static void LogError(string message, Exception ex)
    {
        _logger?.Error(ex, message);
    }
}
