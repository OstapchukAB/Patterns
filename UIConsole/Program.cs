using System;
using FabricMethodClassLib;
namespace UIConsole;
class Program
{
    static void Main()
    {
        Logger.InitLogger(); // Запускаем логирование  .
        IFileService fileService = new FileService();

        while (true)
        {
            Console.WriteLine("\nФайловый менеджер:");
            Console.WriteLine("1 - Просмотреть файлы");
            Console.WriteLine("2 - Прочитать файл");
            Console.WriteLine("3 - Удалить файл");
            Console.WriteLine("0 - Выход");
            Console.Write("Выберите действие: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.Write("Введите путь к папке: ");
                    string dir = Console.ReadLine();
                    try
                    {
                        var files = fileService.GetFiles(dir);
                        Console.WriteLine("Файлы:");
                        foreach (var file in files)
                            Console.WriteLine(file);

                        Logger.LogInfo($"Пользователь просмотрел файлы в папке: {dir}");
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError($"Ошибка при просмотре файлов в папке: {dir}", ex);
                        Console.WriteLine($"Ошибка: {ex.Message}");
                    }
                    break;

                case "2":
                    Console.Write("Введите путь к файлу: ");
                    string filePath = Console.ReadLine();
                    try
                    {
                        string content = fileService.ReadFile(filePath);
                        Console.WriteLine("Содержимое файла:");
                        Console.WriteLine(content);

                        Logger.LogInfo($"Пользователь открыл файл: {filePath}");
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError($"Ошибка при чтении файла: {filePath}", ex);
                        Console.WriteLine($"Ошибка: {ex.Message}");
                    }
                    break;

                case "3":
                    Console.Write("Введите путь к файлу для удаления: ");
                    string deletePath = Console.ReadLine();
                    try
                    {
                        fileService.DeleteFile(deletePath);
                        Console.WriteLine("Файл удалён.");

                        Logger.LogInfo($"Пользователь удалил файл: {deletePath}");
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError($"Ошибка при удалении файла: {deletePath}", ex);
                        Console.WriteLine($"Ошибка: {ex.Message}");
                    }
                    break;

                case "0":
                    Logger.LogInfo("Пользователь завершил работу с файловым менеджером.");
                    return;

                default:
                    Logger.LogInfo($"Пользователь ввёл некорректный выбор: {choice}");
                    Console.WriteLine("Неверный ввод, попробуйте снова.");
                    break;
            }
        }
    }
}
