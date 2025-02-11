namespace OrdersRestaurant.Core.Domain;

public class DineInOrder : Order
{
    public DineInOrder(string description) : base(description)
    {
    }

    public override void ProcessOrder()
    {
         System.Console.WriteLine($"Processing dine -in order: {_description}");
    }
}
