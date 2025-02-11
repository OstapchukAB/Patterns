using OrdersRestaurant.Core.Domain;

namespace OrdersRestaurant.Core.Factories;

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
