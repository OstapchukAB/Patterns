#region  Описание паттерна «Декоратор»
/*
Паттерн «Декоратор» – это структурный паттерн, который позволяет динамически добавлять объектам новую функциональность,
оборачивая их в специальные декораторы.
Декораторы реализуют тот же интерфейс, что и базовый объект, и делегируют ему вызовы, при этом добавляя дополнительное поведение до или после основного вызова.

Когда применять:

- Когда нужно расширить функциональность объектов без изменения их исходного кода.
- Когда требуется комбинировать различные аспекты поведения динамически.
- Когда наследование для расширения не подходит (например, из-за большого количества вариантов комбинаций).

Отличия от других паттернов:

- В отличие от паттерна «Фасад», который упрощает интерфейс сложной подсистемы, декоратор добавляет дополнительное поведение объекту.
- В отличие от паттерна «Стратегия», который позволяет выбирать алгоритм из набора вариантов, декоратор расширяет существующий функционал, «оборачивая» объект.

Недостатки:

- При частом использовании может привести к созданию многих мелких классов-декораторов.
- Сложность отладки может увеличиться, так как поведение объекта зависит от динамически составленной цепочки декораторов.
 */
#endregion
#region Пример реализации на C#
using System;

namespace DecoratorPatternExample
{
    // Компонент: базовый интерфейс
    public interface IComponent
    {
        string Operation();
    }

    // Конкретный компонент
    public class ConcreteComponent : IComponent
    {
        public string Operation() => "Основная функциональность";
    }

    // Абстрактный декоратор, реализующий IComponent
    public abstract class Decorator : IComponent
    {
        protected IComponent _component;

        public Decorator(IComponent component)
        {
            _component = component;
        }

        // По умолчанию просто делегирует вызов
        public virtual string Operation() => _component.Operation();
    }

    // Конкретный декоратор A
    public class ConcreteDecoratorA : Decorator
    {
        public ConcreteDecoratorA(IComponent component) : base(component) { }

        public override string Operation()
        {
            // Дополнительное поведение до вызова базовой операции
            return $"[DecoratorA]({base.Operation()})";
        }
    }

    // Конкретный декоратор B
    public class ConcreteDecoratorB : Decorator
    {
        public ConcreteDecoratorB(IComponent component) : base(component) { }

        public override string Operation()
        {
            // Дополнительное поведение после вызова базовой операции
            return $"{base.Operation()} + [DecoratorB]";
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            IComponent component = new ConcreteComponent();
            Console.WriteLine("Без декораторов: " + component.Operation());

            // Оборачиваем компонент в декоратор A
            IComponent decoratorA = new ConcreteDecoratorA(component);
            Console.WriteLine("С декоратором A: " + decoratorA.Operation());

            // Оборачиваем результат в декоратор B
            IComponent decoratorB = new ConcreteDecoratorB(decoratorA);
            Console.WriteLine("С декораторами A и B: " + decoratorB.Operation());
        }
    }
}

#endregion