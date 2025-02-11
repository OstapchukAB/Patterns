namespace ProceduralExample
{
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
    public abstract class CreatorCar:IMessageCar
    {
        public void DisplayInfo(string name)
        {
            Console.WriteLine($"Выпущен автомобиль:{name}");
        }

        public abstract Car FactoryCar(string name);
        public abstract Car FactoryCar();

    }
    interface IMessageCar
    {
        void DisplayInfo(string name);
    }

    class SedanCar : Car
    {
        public SedanCar(string model) : base(model)
        {
        }
    }
    class SuvCar : Car
    {
        public SuvCar(string model) : base(model)
        {
        }
    }
    class CoupeCar : Car
    {
        public CoupeCar(string model) : base(model)
        {
        }
    }

    class CreatorSedan : CreatorCar
    {
        public override Car FactoryCar(string name)
        {
            var car = new SedanCar(name);
            DisplayInfo(car.Model);
            return car;
        }

        public override Car FactoryCar()
        {
            var car = new SedanCar("Sedan");
            DisplayInfo(car.Model);
            return car;
        }
    }
    class CreatorSuv : CreatorCar
    {
        public override Car FactoryCar(string name)
        {
            var car= new SuvCar(name);
            DisplayInfo(car.Model);
            return car;
        }

        public override Car FactoryCar()
        {
            var car = new SuvCar("SUV");
            DisplayInfo(car.Model);
            return car;
        }
    }
    class CreatorCoupe : CreatorCar
    {
        public override Car FactoryCar(string name)
        {
            var car=new CoupeCar(name);
            DisplayInfo(car.Model);
            return car;
        }

        public override Car FactoryCar()
        {
            var car = new CoupeCar("Coupe");
            DisplayInfo(car.Model);
            return car;
        }
    }

    // Магазин автомобилей, где выбор и создание конкретного автомобиля осуществляется через условные операторы
    class CarStore
    {
        public void SellCar(CreatorCar creator)
        {
            Car? car;
            car = creator.FactoryCar();

            if (car != null) 
            { 
                Console.WriteLine($"Автомобиль {car.Model} продан!");
            }
        }
        public void SellCar(CreatorCar creator,string name)
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
            store.SellCar(new CreatorSuv(),"Suv2025");
        }
    }
}
