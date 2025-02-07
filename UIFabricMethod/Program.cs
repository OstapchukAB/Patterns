using System;
#region Объяснение
//Проблемы данного подхода:

//    Жесткая зависимость от конкретных классов: Создание объектов происходит напрямую в клиентском коде, что затрудняет замену или расширение функционала.
//    Нарушение принципа открытости/закрытости (OCP): При добавлении нового типа продукта придется изменять метод Main, что увеличивает риск внесения ошибок.
//    Отсутствие инкапсуляции логики создания объектов: Логика создания продукта разбросана по клиентскому коду, что снижает читаемость и поддержку кода.
//    Сложность масштабирования: При увеличении количества типов продуктов код с ветвлениями (if/else) становится громоздким и менее поддерживаемым.

//2. Почему применение паттерна "Фабричный метод" необходимо и в чем его преимущества

//Применение фабричного метода позволяет:

//    Инкапсулировать логику создания объектов: Клиентский код не знает о конкретных классах, что упрощает замену реализаций.
//    Снизить связанность: Клиент работает с абстракциями (интерфейсами или абстрактными классами), а не с конкретными реализациями.
//    Соблюдать принцип открытости/закрытости: Добавление нового продукта не требует изменения клиентского кода, достаточно создать новый конкретный класс фабрики.
//    Повысить тестируемость: Логику создания объектов можно централизовать и при необходимости заменить на моки при тестировании.
//    Улучшить масштабируемость: При расширении функционала не увеличивается сложность клиентского кода.
#endregion
#region Решение
//Преимущества фабричного метода:

//    Логика создания объектов теперь инкапсулирована в классах-«фабриках».
//    При добавлении нового типа продукта достаточно создать новый конкретный класс создателя, не изменяя клиентский код.
//    Клиентский код работает с абстрактными типами, что снижает связанность и повышает гибкость системы.
#endregion
namespace FactoryMethod
{
    // Интерфейс продукта
    public interface IProduct
    {
        void DoSomething();
    }
    // Конкретный продукт A
    public class ConcreteProductA : IProduct
    {
        public void DoSomething()
        {
            Console.WriteLine("Product A operation.");
        }
    }
    // Конкретный продукт B
    public class ConcreteProductB : IProduct
    {
        public void DoSomething()
        {
            Console.WriteLine("Product B operation.");
        }
    }
    //************** Применяем паттерн фабричный метод
    //Абстрактный создатель - фабрика
    public abstract class Creator
    {
        // Объявляем фабричный метод - метод возвращающий интерфейс IProduct
        public abstract IProduct FactoryMethod();

        //Некоторая операция использущая продукт
        public void SomeOperation()
        {
            IProduct product = FactoryMethod();
            product.DoSomething();
        }
    }

    //Конкретный создатель А реализующий фабричный метод
    public class ConcreteCreatorA : Creator
    {
        public override IProduct FactoryMethod()
        {
            return new ConcreteProductA();
        }
    }
    //Конкретный создатель B реализующий фабричный метод
    public class ConcreteCreatorB : Creator
    {
        public override IProduct FactoryMethod()
        {
            return new ConcreteProductB();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter product type (A/B):");
            string input = Console.ReadLine();
            Creator creator;  // IProduct product;
            if (input.Equals("A", StringComparison.OrdinalIgnoreCase))
            {
                creator = new ConcreteCreatorA();   //product = new ConcreteProductA();
            }
            else if (input.Equals("B", StringComparison.OrdinalIgnoreCase))
            {
                creator = new ConcreteCreatorB(); // product = new ConcreteProductB();
            }
            else
            {
                Console.WriteLine("Invalid product type.");
                return;
            }
            creator.SomeOperation();  //product.DoSomething();
        }
    }
}
