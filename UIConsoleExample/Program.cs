namespace ProceduralExample
{

    // Абстрактный класс, представляющий автомобиль
    public abstract class Car
    {
        public string Model { get; set; }

        public Car(string model)
        {
            Model = model;
        }

        public void DisplayInfo()
        {
                Console.WriteLine($"Car model: {Model}");
        }
    }

    // Абстрактная фабрика для создания автомобилей
    // Теперь используется один метод CreateCar с необязательным параметром.
    public abstract class CarFactory:IMessageCar
    {
        public void DisplayInfo(string name)
        {
            Console.WriteLine($"Выпущен автомобиль:{name}");
        }

        // Метод создания автомобиля с необязательным параметром.
        // Если имя не передано (null), каждая фабрика сама задаст значение по умолчанию.
        public abstract Car FactoryCar(string? name=null);


    }

    // Интерфейс для вывода сообщения о созданном автомобиле
    interface IMessageCar
    {
        void DisplayInfo(string name);
    }

    public class SedanCar : Car
    {
        public SedanCar(string model) : base(model)
        {
        }
    }
    public class SuvCar : Car
    {
        public SuvCar(string model) : base(model)
        {
        }
    }
    public class CoupeCar : Car
    {
        public CoupeCar(string model) : base(model)
        {
        }
    }

    public class SedanFactory : CarFactory
    {
        public override Car FactoryCar(string? name=null)
        {
            
            var car = new SedanCar(name ??"Sedan");
            DisplayInfo(car.Model);
            return car;
        }

    }
    public class SuvFactory : CarFactory
    {
        public override Car FactoryCar(string? name=null)
        {
            var car= new SuvCar(name ?? "SUV");
            DisplayInfo(car.Model);
            return car;
        }
    }
    public class CoupeFactory : CarFactory
    {
        public override Car FactoryCar(string? name=null)
        {
            var car=new CoupeCar(name ?? "Coupe" );
            DisplayInfo(car.Model);
            return car;
        }
    }

    // Магазин автомобилей, где выбор и создание конкретного автомобиля осуществляется через условные операторы
    public class CarStore
    {
        public void SellCar(CarFactory creator,string? name =null)
        {
            Car? car;
            car = creator.FactoryCar(name);

            if (car != null)
            {
                Console.WriteLine($"Автомобиль {car.Model} продан!");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            CarStore store = new CarStore();
            store.SellCar(new SuvFactory());
            store.SellCar(new SedanFactory());
        }
    }
}
