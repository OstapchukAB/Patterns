namespace OrdersRestaurant.Core.Domain;

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
