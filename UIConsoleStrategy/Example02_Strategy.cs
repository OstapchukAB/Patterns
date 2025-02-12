using System.Text;

namespace TravelCardes;
#region Задание
/*
Реализовать консольное приложение, в котором используется паттерн "Стратегия" для вычисления стоимости проезда на общественном транспорте.
Требования:

    Есть три стратегии расчёта стоимости проезда:
        Обычный тариф – фиксированная цена (например, 50 рублей).
        Студенческий тариф – скидка 30% от обычного тарифа.
        Проездной билет – фиксированная месячная стоимость (например, 2000 рублей), при этом каждая поездка считается бесплатной.

    Контекст — это класс FareCalculator, который принимает стратегию и вычисляет стоимость поездки.

    Приложение должно позволять пользователю:
        Выбрать нужную стратегию тарифа.
        Ввести количество поездок в месяц.
        Рассчитать итоговую стоимость проезда.


 */
#endregion
public interface IStrategy
{
    public decimal ExecuteStrategy(int numberOfTripsPerMonth);

}

public abstract class Fare
{
    protected bool Unlimited=false;
    int _numberOfTripsPerMonth;
    public int NumberOfTripsPerMonth
    {
        get => _numberOfTripsPerMonth;
        protected set
        {
            if (value >= 0)
            {
                _numberOfTripsPerMonth = value;
            }
            else
            {
                throw new InvalidOperationException("количество поездок не может быть отрицательным");
            }
        }
    }


    decimal _unitCost;
    public decimal UnitCost
    {
        get => _unitCost;
        protected set 
        {
            if (value >= 0) 
            { 
                _unitCost = value;
            }
            else
            {
                throw new InvalidOperationException("стоимость не может быть отрицательная");
            }
        }
    
    }
    public string Comment { get; set; } = "";
}

public class FixFare : Fare, IStrategy
{
    public FixFare()
    {
        UnitCost = 50m;
    }

    public decimal ExecuteStrategy(int numberOfTripsPerMonth)
    {
        NumberOfTripsPerMonth = numberOfTripsPerMonth;
        return NumberOfTripsPerMonth * UnitCost;
    }
}
public class StudentFare : Fare, IStrategy
{
    public StudentFare()
    {
        UnitCost = new FixFare().ExecuteStrategy(1) * 0.7m;
    }

    public decimal ExecuteStrategy(int numberOfTripsPerMonth)
    {
        NumberOfTripsPerMonth = numberOfTripsPerMonth;
        return NumberOfTripsPerMonth * UnitCost;
    }
}

public class TravelTicketFare : Fare, IStrategy
{

    public TravelTicketFare()
    {
        Unlimited=true;
        UnitCost = 2000m;
        Comment = "The cost of each trip during the month is free of chage";
    }

    public decimal ExecuteStrategy(int numberOfTripsPerMonth)
    {
        return UnitCost;
    }
}

public class FareCalculatorContext
{
    public IStrategy _strategy;

    public FareCalculatorContext(IStrategy strategy)
    {
        _strategy = strategy;
    }

    public void SetStrategy(IStrategy strategy)
    {
        _strategy = strategy;
    }
    public decimal GetTotalCost(int numberOfTripsPerMonth)
    {
        
        return _strategy.ExecuteStrategy(numberOfTripsPerMonth == 0 ?1: numberOfTripsPerMonth);
    }
}

public class Program
{
    public static void Main()
    {

        Console.OutputEncoding = Encoding.UTF8;

        Console.WriteLine("Выберите тариф: FixPrice - 1 Sudent - 2 TravelTicket - 3");
        var tariff = int.Parse(Console.ReadLine());
        Console.WriteLine("Выберите количество поездок в месяц");
        var numberOfTripsPerMonth = int.Parse(Console.ReadLine());

        Fare fare;
        FareCalculatorContext calculator;
        switch (tariff)
        {
            case 1:
                Console.WriteLine("FIX");
                fare = new FixFare();
                calculator = new((FixFare)fare);
                Console.WriteLine("FixPrice. Unit Cost: {0:C}  Number of trips per month: {1}  Total Cost: {2:C}",
                    ((Fare)calculator._strategy).UnitCost, numberOfTripsPerMonth,
                    calculator.GetTotalCost(numberOfTripsPerMonth));
                break;

            case 2:
                Console.WriteLine("STUDENT");
                fare = new StudentFare();
                calculator = new((StudentFare)fare);
                Console.WriteLine("Student. Unit Cost: {0:C}  Number of trips per month: {1}  Total Cost: {2:C}", ((Fare)calculator._strategy).UnitCost, numberOfTripsPerMonth, calculator.GetTotalCost(numberOfTripsPerMonth));
                break;

            case 3:
                Console.WriteLine("TRAVEL");
                fare = new TravelTicketFare();
                calculator = new((TravelTicketFare)fare);
                Console.WriteLine("TravelTicket. Unit Cost: {0:C}  Number of trips per month: {1}  Total Cost: {2:C} Comment {3}",
                    ((Fare)calculator._strategy).UnitCost,
                    numberOfTripsPerMonth,
                    calculator.GetTotalCost(numberOfTripsPerMonth),
                    fare.Comment
                    );
                break;

            default:
                Console.WriteLine("нет такого тарифа");
                break;
        }









    }
}

