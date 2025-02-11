using OrdersRestaurant.Core.Domain;

namespace OrdersRestaurant.Core.Factories;
/// <summary>
/// фабрика создания заказов на вынос
/// </summary>
public class FactoryTakeawayOrder : FactoryOrders
{
    public override Order FactoryCreateOrder(string description)
    {
        return new TakeawayOrder(description);
    }
}
