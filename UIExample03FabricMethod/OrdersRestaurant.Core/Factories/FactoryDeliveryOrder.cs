using OrdersRestaurant.Core.Domain;

namespace OrdersRestaurant.Core.Factories;

public class FactoryDeliveryOrder : FactoryOrders
{
    public override Order FactoryCreateOrder(string description)
    {
        return new DeliveryOrder(description);
    }
}
