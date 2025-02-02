using Serilog;

namespace MyClassLib
{
    public sealed class FileLogger
    {
        private static readonly Lazy<FileLogger> _instance = new(() => new FileLogger());

        private readonly ILogger _logger;

        private FileLogger()
        {
            _logger = new LoggerConfiguration()
                .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();
        }

        public static FileLogger Instance => _instance.Value;

        public void LogInfo(string message)
        {
            _logger.Information(message);
        }

        public void LogError(string message, Exception ex)
        {
            _logger.Error(ex, message);
        }
    }
}
