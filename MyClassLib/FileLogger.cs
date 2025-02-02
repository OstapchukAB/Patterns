using Serilog;

namespace MyClassLib
{
//	Запрещаем наследование: Класс FileLogger помечен как sealed, чтобы предотвратить наследование.
//	Ленивое создание: Экземпляр синглтона создается лениво, чтобы гарантировать его создание только при необходимости.
//	Приватный конструктор: Конструктор приватный, чтобы предотвратить создание экземпляров извне класса.
//	Настройка логгера: Логгер настроен для записи логов в файл с ежедневным архивированием.
//	Единственный экземпляр: Свойство Instance предоставляет доступ к единственному экземпляру класса.
//	Методы логирования: Методы LogInfo и LogError предназначены для логирования информационных сообщений и ошибок соответственно.


    // Запрещаем наследование от этого класса
    public sealed class FileLogger
    {
        // Ленивое создание единственного экземпляра (синглтон)
        private static readonly Lazy<FileLogger> _instance = new(() => new FileLogger());

        // Экземпляр логгера Serilog
        private readonly ILogger _logger;

        // Приватный конструктор, чтобы предотвратить создание экземпляров извне
        private FileLogger()
        {
            // Настройка логгера для записи в файл с ежедневным архивированием
            _logger = new LoggerConfiguration()
                .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();
        }

        // Публичное свойство для доступа к единственному экземпляру
        public static FileLogger Instance => _instance.Value;

        // Метод для логирования информационных сообщений
        public void LogInfo(string message)
        {
            _logger.Information(message);
        }

        // Метод для логирования ошибок с исключением
        public void LogError(string message, Exception ex)
        {
            _logger.Error(ex, message);
        }
    }
}
