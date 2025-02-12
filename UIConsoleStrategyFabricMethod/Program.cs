using System;
#region Объяснение
/*
 Разбор кода

    Паттерн Стратегия
        IStrategy — интерфейс для расчёта скидки.
        PercentageDiscount, FixedDiscount, NoDiscount — конкретные стратегии расчёта.
        DiscountCalculatorContext — контекст, который применяет стратегию.

    Паттерн Фабричный метод
        DiscountFactory.CreateDiscount(string type) — создаёт нужную стратегию на основе параметра.

******************************************
Преимущества
Преимущества такого подхода

 Гибкость: Легко менять стратегии без изменения основного кода.
 Расширяемость: Можно добавить новые стратегии скидки без модификации существующего кода.
 Принципы SOLID:

    S (Single Responsibility) — каждая стратегия отвечает только за расчёт скидки.
    O (Open/Closed) — можно добавлять новые стратегии, не изменяя код контекста.
    D (Dependency Inversion) — контекст использует интерфейс, а не конкретные классы.

Этот код демонстрирует, как Стратегия + Фабричный метод работают вместе:

    Стратегия даёт гибкость выбора алгоритма (разные скидки).
    Фабричный метод создаёт стратегию динамически.
    Контекст (DiscountCalculatorContext) не зависит от конкретных стратегий.

Это делает систему удобной, гибкой и расширяемой.
**********************************************************************


Представленный пример использует статическую фабрику (также её называют «Simple Factory» или «Static Factory Method»), 
а не классическую реализацию паттерна «Фабричный метод» (Factory Method) из канона GoF.
Разъяснение различий

    Классический паттерн «Фабричный метод»:
    Обычно включает иерархию создателей (Creator), где базовый класс (или интерфейс) объявляет виртуальный или абстрактный метод создания продукта,
а конкретные подклассы (Concrete Creators) переопределяют этот метод для создания конкретного объекта.
Это позволяет динамически выбирать конкретный тип продукта через наследование и полиморфизм.

    Статическая фабрика (Simple Factory):
    В нашем примере класс DiscountFactory содержит статический метод, который по входному параметру возвращает нужный объект,
реализующий интерфейс IStrategy. 
Такой подход проще, но он не демонстрирует динамическое переопределение фабричного метода через наследование.

Почему пример всё же подходит для демонстрации совместного применения паттернов?

В данном случае показано как можно совместить Стратегию с фабричным подходом для создания стратегии.
Хотя использована статическая фабрика, идея остаётся: клиентский код не знает о конкретных реализациях стратегии,
а получает её через метод фабрики. Это соответствует принципам инкапсуляции создания объектов,
что является одной из целей паттерна «Фабричный метод».
 */
#endregion
#region Рекомендуемая структура решения
/*
 DiscountSolution.sln
│
├── Discount.Core
│   ├── Strategies
│   │   ├── IDiscountStrategy.cs          // Интерфейс для стратегии скидки
│   │   ├── FixedDiscount.cs                // Конкретная стратегия: фиксированная скидка
│   │   ├── PercentageDiscount.cs           // Конкретная стратегия: процентная скидка
│   │   └── NoDiscount.cs                   // Конкретная стратегия: без скидки
│   │
│   ├── Factories
│   │   ├── DiscountCreator.cs              // Абстрактный класс-фабрика (Creator)
│   │   ├── FixedDiscountCreator.cs         // Конкретный создатель для FixedDiscount
│   │   ├── PercentageDiscountCreator.cs    // Конкретный создатель для PercentageDiscount
│   │   └── NoDiscountCreator.cs            // Конкретный создатель для NoDiscount
│   │
│   ├── Contexts
│   │   └── DiscountCalculatorContext.cs    // Контекст, использующий стратегию (через фабричный метод)
│   │
│   └── Models
│       └── (Дополнительные классы, если нужны для представления данных)
│
├── Discount.ConsoleApp
│   └── Program.cs                          // Консольное приложение, демонстрирующее работу паттернов
│
└── Discount.Tests
    └── (Юнит-тесты для проверки работы стратегий, фабрик и контекста)

 */
#endregion
namespace StrategyFactoryExample
{
    /// <summary>
    /// Интерфейс стратегии расчёта скидки.
    /// </summary>
    public interface IStrategy
    {
        decimal Calculate(decimal price);
    }

    /// <summary>
    /// Стратегия процентной скидки.
    /// </summary>
    public class PercentageDiscount : IStrategy
    {
        private readonly decimal _percentage;

        public PercentageDiscount(decimal percentage)
        {
            _percentage = percentage;
        }

        public decimal Calculate(decimal price)
        {
            return price - (price * _percentage / 100);
        }
    }

    /// <summary>
    /// Стратегия фиксированной скидки.
    /// </summary>
    public class FixedDiscount : IStrategy
    {
        private readonly decimal _discountAmount;

        public FixedDiscount(decimal discountAmount)
        {
            _discountAmount = discountAmount;
        }

        public decimal Calculate(decimal price)
        {
            return Math.Max(0, price - _discountAmount);
        }
    }

    /// <summary>
    /// Стратегия без скидки.
    /// </summary>
    public class NoDiscount : IStrategy
    {
        public decimal Calculate(decimal price)
        {
            return price; // Цена остаётся неизменной.
        }
    }

    /// <summary>
    /// Фабрика для создания стратегий скидки.
    /// </summary>
    public static class DiscountFactory
    {
        public static IStrategy CreateDiscount(string type)
        {
            return type switch
            {
                "percentage" => new PercentageDiscount(10), // 10% скидка
                "fixed" => new FixedDiscount(50),          // Скидка 50 у.е.
                _ => new NoDiscount()                      // Без скидки
            };
        }
    }

    /// <summary>
    /// Контекст, использующий стратегию скидки.
    /// </summary>
    public class DiscountCalculatorContext
    {
        private IStrategy _strategy;

        public DiscountCalculatorContext(IStrategy strategy)
        {
            _strategy = strategy;
        }

        public void SetStrategy(IStrategy strategy)
        {
            _strategy = strategy;
        }

        public decimal CalculateFinalPrice(decimal price)
        {
            return _strategy.Calculate(price);
        }
    }

    class Program
    {
        //static void Main()
        //{
        //    Console.OutputEncoding = System.Text.Encoding.UTF8;

        //    Console.WriteLine("Введите стоимость товара:");
        //    decimal price = decimal.Parse(Console.ReadLine());

        //    Console.WriteLine("Выберите тип скидки: percentage, fixed, none");
        //    string discountType = Console.ReadLine();

        //    // Используем фабричный метод для получения стратегии скидки.
        //    IStrategy strategy = DiscountFactory.CreateDiscount(discountType);

        //    // Создаём контекст с выбранной стратегией.
        //    DiscountCalculatorContext calculator = new(strategy);

        //    // Рассчитываем итоговую цену.
        //    decimal finalPrice = calculator.CalculateFinalPrice(price);

        //    Console.WriteLine($"Итоговая цена с учётом скидки: {finalPrice:C}");
        //}
    }
}
