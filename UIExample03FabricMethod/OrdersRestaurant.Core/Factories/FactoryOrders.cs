using OrdersRestaurant.Core.Domain;

namespace OrdersRestaurant.Core.Factories;
public abstract class FactoryOrders
{
    /// <summary>
    /// абстрактная фабрика создания заказов
    /// </summary>
    /// <param name="description"></param>
    /// <returns></returns>
    public abstract Order FactoryCreateOrder(string description);

}
