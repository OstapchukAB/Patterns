using System;
#region Описание
/*
 Ключевые моменты реализации:

IStrategy: Интерфейс, определяющий метод Execute(), который реализуется в каждой конкретной стратегии.
ConcreteStrategyA, ConcreteStrategyB, ConcreteStrategyC: Разные реализации интерфейса, каждая из которых реализует свою версию алгоритма.
Context: Класс, который содержит ссылку на текущую стратегию и делегирует ей выполнение задачи через метод ExecuteStrategy().
Также предоставляет метод SetStrategy(), позволяющий сменить стратегию в рантайме.

Данный пример демонстрирует гибкость паттерна «Стратегия»: клиентский код (контекст) может динамически изменять алгоритм,
который используется для решения задачи, без изменения его собственного кода.
 */
#endregion
#region UML-schema
/*
             +---------------------+
            |      Context        |
            |---------------------|
            | - strategy: Strategy|
            |---------------------|
            | + Context(strategy: Strategy)  |
            | + setStrategy(strategy: Strategy)|
            | + executeStrategy()              |
            +---------------------+
                      |
                      |  использует
                      V
            +---------------------+
            |     <<interface>>   |
            |      Strategy       |
            |---------------------|
            | + execute()         |
            +---------------------+
                      ^
                      |
       ---------------------------------
       |               |               |
+--------------+ +--------------+ +--------------+
| ConcreteStrategyA| |ConcreteStrategyB| |ConcreteStrategyC|
|------------------| |-----------------| |-----------------|
| + execute()      | | + execute()     | | + execute()     |
+--------------+ +--------------+ +--------------+

 
 */
#endregion
namespace StrategyPatternExample
{
    // Интерфейс стратегии
    public interface IStrategy
    {
        void Execute();
    }

    // Конкретная стратегия A
    public class ConcreteStrategyA : IStrategy
    {
        public void Execute()
        {
            Console.WriteLine("Executing strategy A: Быстрое выполнение алгоритма A.");
        }
    }

    // Конкретная стратегия B
    public class ConcreteStrategyB : IStrategy
    {
        public void Execute()
        {
            Console.WriteLine("Executing strategy B: Надёжное выполнение алгоритма B.");
        }
    }

    // Конкретная стратегия C
    public class ConcreteStrategyC : IStrategy
    {
        public void Execute()
        {
            Console.WriteLine("Executing strategy C: Экономичное выполнение алгоритма C.");
        }
    }

    // Класс контекста, использующий стратегию
    public class Context
    {
        private IStrategy _strategy;

        // Конструктор принимает начальную стратегию
        public Context(IStrategy strategy)
        {
            _strategy = strategy;
        }

        // Позволяет установить новую стратегию
        public void SetStrategy(IStrategy strategy)
        {
            _strategy = strategy;
        }

        // Выполняет алгоритм, делегируя выполнение стратегии
        public void ExecuteStrategy()
        {
            _strategy.Execute();
        }
    }

    //public static class Program
    //{
    //    public static void Main(string[] args)
    //    {
    //        // Инициализация контекста с ConcreteStrategyA
    //        Context context = new Context(new ConcreteStrategyA());
    //        context.ExecuteStrategy();  // Вывод: Executing strategy A: Быстрое выполнение алгоритма A.

    //        // Замена стратегии на ConcreteStrategyB
    //        context.SetStrategy(new ConcreteStrategyB());
    //        context.ExecuteStrategy();  // Вывод: Executing strategy B: Надёжное выполнение алгоритма B.

    //        // Замена стратегии на ConcreteStrategyC
    //        context.SetStrategy(new ConcreteStrategyC());
    //        context.ExecuteStrategy();  // Вывод: Executing strategy C: Экономичное выполнение алгоритма C.
    //    }
    //}
}
