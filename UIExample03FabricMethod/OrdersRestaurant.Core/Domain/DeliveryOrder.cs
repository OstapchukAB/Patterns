namespace OrdersRestaurant.Core.Domain;

public class DeliveryOrder : Order
{
    public DeliveryOrder(string description) : base(description)
    {
    }
    public override void ProcessOrder()
    {
        System.Console.WriteLine($"Processing delivery order: {_description}");
    }
}
