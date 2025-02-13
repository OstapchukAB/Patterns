using System;
using System.IO;
using System.Linq;

#region Описание
/*
пример процедурного (антипаттерн) кода для обработки файлов. 
В этом примере метод ProcessFile выполняет все действия – определяет тип файла по расширению,
выбирает алгоритм обработки и выполняет его, используя цепочку условных операторов.
Такой подход затрудняет расширение, тестирование и сопровождение кода.
Его можно использовать как отправную точку для последующего рефакторинга с применением паттернов Фабрика и Стратегия.
*****************************
Ключевые моменты данного антипаттерна:

Объединение логики создания и применения алгоритмов:
Метод ProcessFile сам определяет, какую обработку выполнить, что нарушает принцип единственной ответственности.

Жёсткое связывание с типами файлов:
При добавлении нового формата (например, XML или HTML) нужно изменять этот метод, что нарушает принцип открытости/закрытости (OCP).

Отсутствие инкапсуляции алгоритмов:
Логика обработки каждого типа файла распределена внутри одного метода, что затрудняет повторное использование и тестирование отдельных алгоритмов.

Этот код можно использовать как отправную точку для рефакторинга с применением паттернов
Фабрика (для создания обработчиков файлов)
и 
Стратегия (для инкапсуляции алгоритмов обработки),
что позволит добиться более гибкого и расширяемого решения.

*/ 
#endregion
namespace FileProcessingPattern
{
 
    public interface IStrategy
    {
        void ProcesseFile(string filePath);
    }

    public class ProcessTxtFile : IStrategy
    {
        public void ProcesseFile(string filePath)
        {
            // Обработка текстового файла: подсчёт количества строк.
            string[] lines = File.ReadAllLines(filePath);
            Console.WriteLine("Текстовый файл содержит {0} строк.", lines.Length);
        }
    }
    public class ProcessCsvFile : IStrategy
    {
        public void ProcesseFile(string filePath)
        {
            // Обработка CSV-файла: подсчёт количества столбцов в первой строке.
            string[] lines = File.ReadAllLines(filePath);
            if (lines.Length > 0)
            {
                string[] columns = lines[0].Split(';');
                Console.WriteLine("CSV-файл содержит {0} столбцов.", columns.Length);
            }
            else
            {
                Console.WriteLine("CSV-файл пуст.");
            }
        }
    }
    public class ProcessJsonFile : IStrategy
    {
        public void ProcesseFile(string filePath)
        {
            // Обработка JSON-файла: приближённый подсчёт объектов.
            // (Наивный способ – подсчитываем количество символов '{'.)
            string jsonContent = File.ReadAllText(filePath);
            int objectCount = jsonContent.Count(ch => ch == '{');
            Console.WriteLine("JSON-файл содержит приблизительно {0} объектов.", objectCount);
        }
    }

    public class ContextProcessFile
    {
        private IStrategy _strategy;
        public ContextProcessFile(IStrategy strategy)
        {
            _strategy = strategy;
        }
        public void ProcessFile(string filePath)
        {
            _strategy.ProcesseFile(filePath);
        }
    }

    //Простая фабрика получения нужной стратегии
    public static class FabricStrategyProcessFile
    {
        public static IStrategy? CreateStrategy(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine("Файл не существует.");
                return null;
            }

            // Получаем расширение файла в нижнем регистре.
            string extension = Path.GetExtension(filePath).ToLower();

            return extension switch
            {
            ".txt" => new ProcessTxtFile(),
            ".csv" => new ProcessCsvFile(),
            ".json" => new ProcessJsonFile(),
            _ =>null
        
            };
    }
    }

    class Program
    {      
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("Введите путь к файлу:");
            string filePath = Console.ReadLine();

            IStrategy? strategy = FabricStrategyProcessFile.CreateStrategy(filePath);
            if (strategy != null)
            {
                ContextProcessFile context = new ContextProcessFile(strategy);
                context.ProcessFile(filePath);
            }
            else
            {
                Console.WriteLine("Неподдерживаемый тип файла: {0}", Path.GetExtension(filePath));
            }

        }
    }
}
