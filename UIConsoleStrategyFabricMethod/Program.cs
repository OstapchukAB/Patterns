using System;

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
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("Введите стоимость товара:");
            decimal price = decimal.Parse(Console.ReadLine());

            Console.WriteLine("Выберите тип скидки: percentage, fixed, none");
            string discountType = Console.ReadLine();

            // Используем фабричный метод для получения стратегии скидки.
            IStrategy strategy = DiscountFactory.CreateDiscount(discountType);

            // Создаём контекст с выбранной стратегией.
            DiscountCalculatorContext calculator = new(strategy);

            // Рассчитываем итоговую цену.
            decimal finalPrice = calculator.CalculateFinalPrice(price);

            Console.WriteLine($"Итоговая цена с учётом скидки: {finalPrice:C}");
        }
    }
}
