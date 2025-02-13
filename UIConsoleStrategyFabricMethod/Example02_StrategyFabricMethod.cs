#region Начальные условия
/*
Тема: Преобразование текста

Исходные требования (антипаттерн):

    Программа читает строку от пользователя.
    Программа предлагает выбрать тип преобразования:
        Преобразовать в верхний регистр.
        Преобразовать в нижний регистр.
        Реверсировать строку.
    На основании выбранного типа преобразования выполняется нужная операция с помощью условных операторов внутри одного метода.

Задача:

    Рефакторинг: Перепиши исходный код, используя паттерны:
        Стратегия: Для каждого типа преобразования создай отдельную стратегию (класс), реализующую общий интерфейс.
        Фабричный метод: Создай фабрику (либо абстрактного создателя, либо простую статическую фабрику),
которая будет возвращать нужную стратегию на основании выбранного типа преобразования.

    Контекст: Реализуй класс-контекст, который будет принимать стратегию и делегировать ей выполнение преобразования текста.

    Демонстрация работы: В методе Main запроси у пользователя строку и тип преобразования, 
затем используй фабрику для создания стратегии, передай её в контекст и выведи результат.
 */
#endregion
#region Что нужно сделать
/*
 Твоя задача:

    Реализуй интерфейс стратегии (например, ITextTransformationStrategy) с методом для преобразования текста.

    Создай конкретные стратегии:
        UpperCaseStrategy – преобразует строку в верхний регистр.
        LowerCaseStrategy – преобразует строку в нижний регистр.
        ReverseStringStrategy – переворачивает строку (реверс).

    Реализуй фабрику (например, класс TextTransformationStrategyFactory),
которая по входящему типу (например, строке "upper", "lower", "reverse") возвращает нужную стратегию.

    Создай класс-контекст (например, TextTransformer), который принимает стратегию (через конструктор или метод) и делегирует ей выполнение преобразования.

    В методе Main:
        Запроси у пользователя исходную строку и тип преобразования.
        Используй фабрику для создания соответствующей стратегии.
        Передай стратегию в контекст и получи преобразованный текст.
        Выведи результат в консоль.
 */
#endregion
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace TextTransformationExample
{
    #region Стратегии

    /// <summary>
    /// Интерфейс для стратегии преобразования текста.
    /// </summary>
    public interface ITextTransformStrategy
    {
        string TransformText(string text);
    }

    public class TextToUpperStrategy : ITextTransformStrategy
    {
        public string TransformText(string text) => text.ToUpper();
    }

    public class TextToLowerStrategy : ITextTransformStrategy
    {
        public string TransformText(string text) => text.ToLower();
    }

    public class TextToReverseStrategy : ITextTransformStrategy
    {
        public string TransformText(string text)
        {
            char[] arr = text.ToCharArray();
            Array.Reverse(arr);
            return new string(arr);
        }
    }

    /// <summary>
    /// Декоратор для добавления логирования к стратегии преобразования.
    /// </summary>
    public class LoggingTextTransformStrategyDecorator : ITextTransformStrategy
    {
        private readonly ITextTransformStrategy _innerStrategy;

        public LoggingTextTransformStrategyDecorator(ITextTransformStrategy innerStrategy)
        {
            _innerStrategy = innerStrategy;
        }

        public string TransformText(string text)
        {
            Console.WriteLine("Лог: Начало преобразования.");
            string result = _innerStrategy.TransformText(text);
            Console.WriteLine("Лог: Завершено преобразование.");
            return result;
        }
    }

    /// <summary>
    /// Контекст, использующий стратегию преобразования.
    /// </summary>
    public class TextContext
    {
        private ITextTransformStrategy _strategy;

        public TextContext(ITextTransformStrategy strategy)
        {
            _strategy = strategy;
        }

        public void SetStrategy(ITextTransformStrategy strategy)
        {
            _strategy = strategy;
        }

        public string ExecuteAlgorithm(string text)
        {
            return _strategy.TransformText(text);
        }
    }

    #endregion

    #region Фабричный метод

    /// <summary>
    /// Абстрактный класс-фабрика для создания стратегии преобразования текста.
    /// </summary>
    public abstract class TextTransformStrategyCreator
    {
        public abstract ITextTransformStrategy CreateStrategy(string transformationType, int textLength);
    }

    /// <summary>
    /// Конкретная фабрика, создающая стратегии с дополнительной логикой:
    /// Если длина текста превышает 100 символов, стратегия оборачивается в декоратор логирования.
    /// </summary>
    public class ConcreteTextTransformStrategyCreator : TextTransformStrategyCreator
    {
        public override ITextTransformStrategy CreateStrategy(string transformationType, int textLength)
        {
            // Выбираем базовую стратегию по типу
            ITextTransformStrategy strategy = transformationType switch
            {
                "upper" => new TextToUpperStrategy(),
                "lower" => new TextToLowerStrategy(),
                "reverse" => new TextToReverseStrategy(),
                _ => throw new ArgumentException("Неподдерживаемый тип преобразования")
            };

            // Если текст длинный, добавляем логирование
            if (textLength > 100)
            {
                strategy = new LoggingTextTransformStrategyDecorator(strategy);
            }
            return strategy;
        }
    }

    #endregion

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            // Запрос исходного текста
            Console.WriteLine("Введите строку для преобразования:");
            string input = Console.ReadLine();

            // Запрос типа преобразования
            Console.WriteLine("Выберите тип преобразования (upper, lower, reverse):");
            string type = Console.ReadLine();

            // Создаем фабрику, которая возьмет на себя решение, какую стратегию создать,
            // с учетом дополнительного условия (длина текста)
            TextTransformStrategyCreator creator = new ConcreteTextTransformStrategyCreator();
            ITextTransformStrategy strategy = creator.CreateStrategy(type, input.Length);

            // Передаем стратегию в контекст и выполняем преобразование
            TextContext context = new TextContext(strategy);
            string result = context.ExecuteAlgorithm(input);
            Console.WriteLine("Результат: " + result);
        }
    }
}
