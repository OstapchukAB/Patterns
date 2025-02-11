#region Условие задачи
/*
 Задача на реализацию паттерна «Фабричный метод»

Тема: Система обработки заказов еды в ресторане
Требования:

    У тебя есть разные типы заказов:
        DineInOrder (заказ для еды в ресторане)
        TakeawayOrder (заказ на вынос)
        DeliveryOrder (доставка)

    У каждого заказа должен быть метод ProcessOrder(), который выводит информацию о заказе в консоль.

    Используй паттерн «Фабричный метод»:
        Определи абстрактный класс или интерфейс фабрики, который будет создавать заказы.
        Создай конкретные фабрики для каждого типа заказа.

    Главный метод Main должен демонстрировать работу — создай заказы через фабрики и обработай их.

Пример использования (примерный вывод в консоли):

Processing dine-in order: Table #5
Processing takeaway order: Order #1234
Processing delivery order: Address 221B Baker Street
 */
#endregion
namespace OrdersResraurant;
public abstract class Order
{
    protected readonly string _description;

    public Order(string description)
    {
        _description = description;
    }
    /// <summary>
    /// Информация о заказе
    /// </summary>
    public abstract void ProcessOrder();
}
public class DineInOrder : Order
{
    public DineInOrder(string description) : base(description)
    {
    }

    public override void ProcessOrder()
    {
        Console.WriteLine($"Processing dine -in order: {_description}");
    }
}
public class TakeawayOrder : Order
{
    public TakeawayOrder(string description) : base(description)
    {
    }
    public override void ProcessOrder()
    {
        Console.WriteLine($"Processing takeaway order: {_description}");
    }
}
public class DeliveryOrder : Order
{
    public DeliveryOrder(string description) : base(description)
    {
    }
    public override void ProcessOrder()
    {
        Console.WriteLine($"Processing delivery order: {_description}");
    }
}


public abstract class FactoryOrders
{
    /// <summary>
    /// абстрактная фабрика создания заказов
    /// </summary>
    /// <param name="description"></param>
    /// <returns></returns>
    public abstract Order FactoryCreateOrder(string description);
    
}

/// <summary>
/// фабрика создания обедов
/// </summary>
public class FactoryDineInOrder : FactoryOrders
{
    public override Order FactoryCreateOrder(string description)
    {
        return new DineInOrder(description);
    }
}
/// <summary>
/// фабрика создания заказов на вынос
/// </summary>
public class FactoryTakeawayOrder : FactoryOrders
{
    public override Order FactoryCreateOrder(string description)
    {
        return  new TakeawayOrder(description);
    }
}
public class FactoryDeliveryOrder : FactoryOrders
{
    public override Order FactoryCreateOrder(string description)
    {
        return new DeliveryOrder(description);
    }
}

public static class Program
{
    static void Main()
    {
        FactoryOrders factoryOrders= new FactoryDineInOrder();
        factoryOrders.FactoryCreateOrder("Table #5").ProcessOrder();
        
        factoryOrders= new FactoryTakeawayOrder();
        factoryOrders.FactoryCreateOrder("Order #1234").ProcessOrder();

        factoryOrders= new FactoryDeliveryOrder();
        factoryOrders.FactoryCreateOrder("Address 221B Baker Street").ProcessOrder();
        

    }
}