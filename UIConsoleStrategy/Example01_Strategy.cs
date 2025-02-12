#region Задание
/*
 Задача:

Перепиши данный код с использованием паттерна «Стратегия».

    Определи интерфейс (или абстрактный класс) для стратегии вычисления скидки.
    Реализуй несколько конкретных стратегий (например, для скидки "Seasonal", "Clearance", "Loyalty" и "None").
    Создай контекст, который будет принимать выбранную стратегию и использовать её для вычисления итоговой суммы.
    Демонстрируй работу новой реализации в методе Main.

Попробуй реализовать паттерн «Стратегия» для этого примера, а затем пришли свой код для проверки.
 */
#endregion
#region Почему представленный код нарушает принципы SOLID
/*
 Давай разберём, какие проблемы и нарушения принципов SOLID возникают в представленном процедурном коде для вычисления скидки:
1. Нарушение принципа открытости/закрытости (Open/Closed Principle, OCP)

    Суть OCP:
    Модуль должен быть открыт для расширения, но закрыт для модификации. 
    Это означает, что при добавлении нового функционала (например, нового типа скидки) не должно требоваться изменение уже существующего кода.

    Как нарушается:
    В твоем методе CalculateDiscount для каждого нового типа скидки нужно добавить новый блок else if.
    Если появится новый тип скидки, придется изменять код существующего метода. 
    Это приводит к тому, что класс становится хрупким и менее масштабируемым, поскольку любое изменение может повлиять на уже работающую логику.

2. Нарушение принципа единственной ответственности (Single Responsibility Principle, SRP)

    Суть SRP:
    Каждый класс или метод должен иметь одну и только одну причину для изменения – то есть одну ответственность.

    Как нарушается:
    Метод CalculateDiscount отвечает сразу за два аспекта:
        Определение типа скидки на основе входного параметра.
        Вычисление итоговой суммы после применения скидки.

    Если логика расчёта скидки или правила определения скидки изменятся, придется изменять этот метод.
    То есть, он выполняет сразу несколько обязанностей – это затрудняет поддержку и тестирование кода.

3. Другие замечания

    Гибкость и расширяемость:
    Текущая реализация не позволяет легко добавить новые типы скидок без риска повредить уже работающую логику,
    так как изменения в одном месте могут привести к ошибкам в другом.

    Поддерживаемость:
    Длинная цепочка условных операторов усложняет чтение и понимание кода, особенно когда логика становится более сложной.

Вывод

Рефакторинг этого кода с использованием паттерна «Стратегия» позволит:

    Расширять функциональность без модификации существующего кода (OCP).
    Разделить ответственность между классами, каждый из которых отвечает за свою стратегию расчёта скидки (SRP).
    Улучшить читаемость и поддерживаемость кода.

Таким образом, применение паттерна «Стратегия» поможет решить эти проблемы, сделав систему более гибкой и адаптируемой к изменениям.
 */
#endregion

namespace DiscountExample;


public interface IStrategy
{
    decimal CalculateDiscount();
}

public class DiscountSeasonal : IStrategy
{
   public decimal CalculateDiscount() => 0.9m;
   
}
public class DiscountClearance : IStrategy
{
    public decimal CalculateDiscount() => 0.8m;
}
public class DiscountLoyalty : IStrategy
{
    public decimal CalculateDiscount() => 0.85m;
}
public class DiscountNone : IStrategy
{
    public decimal CalculateDiscount() => 1;
}

// Класс для вычисления скидки в процедурном стиле
public class DiscountCalculatorContext:IAmount
{

    IStrategy _strategy;
    decimal _discount;
    public DiscountCalculatorContext(IStrategy strategy)
    {
        _strategy = strategy;
    }

    public decimal CalculateAmount(decimal amount)
    {
        return _discount * amount;
    }

    public void ExecuteStrategy()
    {
        _discount= _strategy.CalculateDiscount();
    }
    public void SetStrategy(IStrategy strategy)
    {
        _strategy = strategy;
    }

}

public interface IAmount
{
    decimal CalculateAmount(decimal amount);
}

class Program
{
    static void Main(string[] args)
    {
        
        decimal originalAmount = 100.00m;
        
        DiscountCalculatorContext calculator = new(new DiscountNone());
        calculator.ExecuteStrategy();
        var amount=calculator.CalculateAmount(originalAmount);
        Console.WriteLine("None discount: {0}", amount);

        calculator.SetStrategy(new DiscountSeasonal());
        calculator.ExecuteStrategy();
        amount = calculator.CalculateAmount(originalAmount);
        Console.WriteLine("Seasonal discount: {0}",amount);

        calculator.SetStrategy(new DiscountClearance());
        calculator.ExecuteStrategy();
        amount = calculator.CalculateAmount(originalAmount);
        Console.WriteLine("Clearance discount: {0}",amount);

        calculator.SetStrategy(new DiscountLoyalty());
        calculator.ExecuteStrategy();
        amount = calculator.CalculateAmount(originalAmount);
        Console.WriteLine("Loyalty discount: {0}",amount);

    }
}



