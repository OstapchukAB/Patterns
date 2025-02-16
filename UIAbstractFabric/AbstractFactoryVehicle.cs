namespace UIAbstractFabricAbstractFactoryVehicle;
    // Интерфейс двигателя
    public interface IEngine
    {
        string GetDescription();
    }

    // Интерфейс шасси
    public interface IChassis
    {
        string GetDescription();
    }

    // Грузовой двигатель
    public class FreightEngine : IEngine
    {
        public string GetDescription() => "Мощный двигатель грузового автомобиля (500 л.с.)";
    }

    // Легковой двигатель
    public class PassengerEngine : IEngine
    {
        public string GetDescription() => "Экономичный двигатель легкового автомобиля (150 л.с.)";
    }

    // Грузовое шасси
    public class FreightChassis : IChassis
    {
        public string GetDescription() => "Шасси с высокой грузоподъемностью";
    }

    // Легковое шасси
    public class PassengerChassis : IChassis
    {
        public string GetDescription() => "Комфортное шасси с хорошей подвеской";
    }

    // Абстрактная фабрика для создания компонентов автомобиля
    public interface IVehicleFactory
    {
        IEngine CreateEngine();
        IChassis CreateChassis();
    }

    // Фабрика для грузовых автомобилей
    public class FreightVehicleFactory : IVehicleFactory
    {
        public IEngine CreateEngine() => new FreightEngine();
        public IChassis CreateChassis() => new FreightChassis();
    }

    // Фабрика для легковых автомобилей
    public class PassengerVehicleFactory : IVehicleFactory
    {
        public IEngine CreateEngine() => new PassengerEngine();
        public IChassis CreateChassis() => new PassengerChassis();
    }

    // Клиентский класс, собирающий автомобиль
    public class VehicleAssembler
    {
        private readonly IVehicleFactory _factory;

        public VehicleAssembler(IVehicleFactory factory)
        {
            _factory = factory;
        }

        public void AssembleVehicle()
        {
            var engine = _factory.CreateEngine();
            var chassis = _factory.CreateChassis();

            Console.WriteLine("Сборка автомобиля завершена. Характеристики деталей:");
            Console.WriteLine("Двигатель: " + engine.GetDescription());
            Console.WriteLine("Шасси: " + chassis.GetDescription());
        }
    }

    public class Program
    {
        public static void Main()
        {
            Console.WriteLine("Выберите тип автомобиля (freight/passenger):");
            string input = Console.ReadLine()?.ToLower();

            IVehicleFactory factory = input switch
            {
                "freight" => new FreightVehicleFactory(),
                "passenger" => new PassengerVehicleFactory(),
                _ => throw new ArgumentException("Неверный тип автомобиля")
            };

            VehicleAssembler assembler = new VehicleAssembler(factory);
            assembler.AssembleVehicle();
        }
    }