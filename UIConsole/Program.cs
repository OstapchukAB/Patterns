using System;
using MyClassLib;
namespace UIConsole;
class Program
{
    static void Main()
    {
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
                    }
                    catch (Exception ex)
                    {
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
                    }
                    catch (Exception ex)
                    {
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
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Ошибка: {ex.Message}");
                    }
                    break;

                case "0":
                    return;

                default:
                    Console.WriteLine("Неверный ввод, попробуйте снова.");
                    break;
            }
        }
    }
}
