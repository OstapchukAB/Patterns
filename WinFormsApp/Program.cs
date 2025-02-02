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

        // Мы передаем второй аргумент как false в Application.ThreadException += (sender, e) => HandleException(e.Exception, false);
        // потому что исключения, обрабатываемые этим обработчиком, не считаются критическими для всего приложения.
        //Объяснение:
        //Application.ThreadException: Этот обработчик предназначен для обработки исключений, которые происходят в потоках Windows Forms.
        //Такие исключения могут быть вызваны ошибками в пользовательском интерфейсе или других частях приложения, работающих в основном потоке.
        //	false для isFatal: Передача false указывает на то, что эти исключения не являются критическими и не требуют немедленного завершения приложения.
        //	Это позволяет приложению продолжать работу после обработки исключения.
        //Пример:
        //        Если пользователь нажимает кнопку, и в обработчике события происходит исключение, это исключение будет обработано Application.ThreadException,
        //        и приложение сможет продолжить работу после отображения сообщения об ошибке.
        //Контраст с AppDomain.CurrentDomain.UnhandledException:
        //	AppDomain.CurrentDomain.UnhandledException: Этот обработчик предназначен для обработки необработанных исключений, которые происходят в любом потоке приложения и
        //	не были пойманы другими обработчиками. Такие исключения считаются критическими.
        //	true для isFatal: Передача true указывает на то, что эти исключения являются критическими и требуют немедленного завершения приложения.
        //Пример:
        //        Если происходит исключение, которое не было поймано ни одним обработчиком, оно будет обработано AppDomain.CurrentDomain.UnhandledException,
        //        и приложение завершится после отображения сообщения об ошибке.




        // Глобальная обработка необработанных исключений для домена приложения
        AppDomain.CurrentDomain.UnhandledException += (sender, e) => HandleException((Exception)e.ExceptionObject, true);

        // Инициализация конфигурации приложения
        ApplicationConfiguration.Initialize();
        // Запуск главной формы приложения
        try
        {
            Application.Run(new MainForm());
        }
        catch (Exception ex)
        {
            HandleException(ex, true);
        }
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
            Environment.Exit(1); // Принудительно завершаем приложение
        }
    }
}
