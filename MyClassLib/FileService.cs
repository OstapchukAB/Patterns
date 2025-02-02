using System;
using System.IO;

namespace MyClassLib
{
    public interface IFileService
    {
        string[] GetFiles(string directoryPath);
        string ReadFile(string filePath);
        void DeleteFile(string filePath);
    }

    public class FileService:IFileService
    {
        // Получить список файлов в папке
        public string[] GetFiles(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
                throw new DirectoryNotFoundException($"Папка '{directoryPath}' не найдена.");

            return Directory.GetFiles(directoryPath);
        }

        // Прочитать содержимое файла
        public string ReadFile(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"Файл '{filePath}' не найден.");

            return File.ReadAllText(filePath);
        }

        // Удалить файл
        public void DeleteFile(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"Файл '{filePath}' не найден.");

            File.Delete(filePath);
        }
    }
}
