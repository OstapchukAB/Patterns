namespace OrdersRestaurant.Core.Domain;
public class TakeawayOrder : Order
{
    public TakeawayOrder(string description) : base(description)
    {
    }
    public override void ProcessOrder()
    {
        System.Console.WriteLine($"Processing takeaway order: {_description}");
    }
}
