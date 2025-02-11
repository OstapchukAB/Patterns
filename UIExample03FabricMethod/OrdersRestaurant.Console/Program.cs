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
#region Распределение кода по проекту
/*
 OrdersRestaurant.sln
│
├── OrdersRestaurant.Core          // Библиотека с бизнес-логикой (классы домена и фабрики)
│   ├── Domain                     // Доменные классы (сущности заказа)
│   │   ├── Order.cs               // Абстрактный класс заказа
│   │   ├── DineInOrder.cs         // Заказ для еды в ресторане
│   │   ├── TakeawayOrder.cs       // Заказ на вынос
│   │   └── DeliveryOrder.cs       // Заказ с доставкой
│   │
│   ├── Factories                  // Фабрики для создания заказов
│   │   ├── FactoryOrders.cs       // Абстрактная фабрика заказов
│   │   ├── FactoryDineInOrder.cs  // Фабрика создания заказов для ресторана
│   │   ├── FactoryTakeawayOrder.cs// Фабрика создания заказов на вынос
│   │   └── FactoryDeliveryOrder.cs// Фабрика создания заказов с доставкой
│   │
│   └── (Дополнительные модули)     // Например, сервисы, валидация и т.д.
│
└── OrdersRestaurant.Console       // Консольное приложение для демонстрации работы библиотеки
    └── Program.cs                 // Точка входа, где демонстрируется использование фабрик

 */
#endregion

using OrdersRestaurant.Core.Factories;

namespace OrdersRestaurant.Console;

public static class Program
{
    static void Main()
    {
        FactoryOrders factoryOrders = new FactoryDineInOrder();
        factoryOrders.FactoryCreateOrder("Table #5").ProcessOrder();

        factoryOrders = new FactoryTakeawayOrder();
        factoryOrders.FactoryCreateOrder("Order #1234").ProcessOrder();

        factoryOrders = new FactoryDeliveryOrder();
        factoryOrders.FactoryCreateOrder("Address 221B Baker Street").ProcessOrder();
    }
}