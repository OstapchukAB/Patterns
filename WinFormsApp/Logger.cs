using Microsoft.VisualBasic.Logging;
using Serilog;

namespace WinFormsApp
{
    public static class Logger
    {
        private static ILogger _logger;

    //	Метод InitLogger: Метод InitLogger инициализирует логгер, настраивая его для записи логов в файл и RichTextBox.
    //	Запись в файл: Логи записываются в файл logs/winforms_log.txt с ежедневным архивированием.
    //	Запись в RichTextBox: Логи также выводятся в RichTextBox для отображения в интерфейсе приложения.
        public static void InitLogger(RichTextBox outputBox)
        {
            _logger = new LoggerConfiguration()
                 .MinimumLevel.Debug()
                .WriteTo.File("logs/winforms_log.txt", rollingInterval: RollingInterval.Day)
                .WriteTo.RichTextBox(outputBox, Serilog.Events.LogEventLevel.Information)
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
        public static void LogFatal(string message, Exception ex)
        {
            _logger?.Fatal(ex, message);
        }
    }
}

