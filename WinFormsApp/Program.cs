namespace WinFormsApp;

static class Program
{
    [STAThread]
    static void Main()
    {
        // Включаем визуальные стили для приложения
        Application.EnableVisualStyles();
        // Устанавливаем совместимость с рендерингом текста по умолчанию
        Application.SetCompatibleTextRenderingDefault(false);

        // Глобальная обработка исключений для потоков Windows Forms
        Application.ThreadException += (sender, e) => HandleException(e.Exception, false);
        // Глобальная обработка необработанных исключений для домена приложения
        AppDomain.CurrentDomain.UnhandledException += (sender, e) => HandleException((Exception)e.ExceptionObject, true);

        // Инициализация конфигурации приложения
        ApplicationConfiguration.Initialize();
        // Запуск главной формы приложения
        Application.Run(new MainForm());
    }

    // Метод для обработки исключений
    private static void HandleException(Exception ex, bool isFatal)
    {
        // Сообщение об ошибке в зависимости от критичности
        string message = isFatal ? "Критическая ошибка приложения!" : "Произошла ошибка.";
        // Логирование критической ошибки
        Logger.LogFatal(message, ex);

        // Показ сообщения об ошибке пользователю
        MessageBox.Show($"{message}\n\n{ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);

        // Закрываем приложение при критической ошибке
        if (isFatal)
        {
            Application.Exit();
        }
    }
}
