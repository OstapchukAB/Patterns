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
public interface IFareStrategy
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

public class FixFare : Fare, IFareStrategy
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
public class StudentFare : Fare, IFareStrategy
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

public class TravelTicketFare : Fare, IFareStrategy
{

    public TravelTicketFare()
    {
        Unlimited=true;
        UnitCost = 2000m;
        Comment = "The cost of each trip during the month is free of chage";
    }

    public decimal ExecuteStrategy(int numberOfTripsPerMonth)
    {
        NumberOfTripsPerMonth = numberOfTripsPerMonth;
        return UnitCost;
    }
}

public class FareCalculatorContext
{
    public IFareStrategy _strategy;

    public FareCalculatorContext(IFareStrategy strategy)
    {
        _strategy = strategy;
    }

    public void SetStrategy(IFareStrategy strategy)
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

        // Настройка кодировки для корректного отображения символов в консоли.
        Console.OutputEncoding = Encoding.UTF8;

        Console.WriteLine("Выберите тариф: FixFare - 1, StudentFare - 2, TravelTicketFare - 3");
        int tariff = int.Parse(Console.ReadLine() ?? "0");
        Console.WriteLine("Введите количество поездок в месяц:");
        int numberOfTrips = int.Parse(Console.ReadLine() ?? "0");

        FareCalculatorContext? calculator = tariff switch
        {
            1 => new (new FixFare()),
            2 => new (new StudentFare()),
            3 => new (new TravelTicketFare()),
            _ => null
        };

        if (calculator == null)
        {
            Console.WriteLine("Нет такого тарифа");
            return;
        }

        decimal totalCost = calculator.GetTotalCost(numberOfTrips);

        // Вывод результатов с использованием стратегии.
        if (tariff == 3)
        {
            // Для проездного билета выводим также комментарий
            TravelTicketFare ticketFare = (TravelTicketFare)calculator._strategy;
            Console.WriteLine("TravelTicketFare: Unit Cost: {0:C}, Количество поездок: {1}, Общая стоимость: {2:C}. {3}",
                ticketFare.UnitCost, numberOfTrips, totalCost, ticketFare.Comment);
        }
        else if (tariff == 1)
        {
            FixFare fixFare = (FixFare)calculator._strategy;
            Console.WriteLine("FixFare: Unit Cost: {0:C}, Количество поездок: {1}, Общая стоимость: {2:C}",
                fixFare.UnitCost, numberOfTrips, totalCost);
        }
        else if (tariff == 2)
        {
            StudentFare studentFare = (StudentFare)calculator._strategy;
            Console.WriteLine("StudentFare: Unit Cost: {0:C}, Количество поездок: {1}, Общая стоимость: {2:C}",
                studentFare.UnitCost, numberOfTrips, totalCost);
        }
    }
}

