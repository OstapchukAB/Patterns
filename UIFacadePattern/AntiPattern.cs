namespace SmartHomeAntiPattern
{
    // Подсистема 1: Освещение
    public class LightSystem
    {
        public void TurnOn() => Console.WriteLine("Освещение включено.");
        public void TurnOff() => Console.WriteLine("Освещение выключено.");
        public void SetBrightness(int level) => Console.WriteLine($"Яркость установлена на {level}%.");
    }

    // Подсистема 2: Кондиционер
    public class AirConditioner
    {
        public void TurnOn() => Console.WriteLine("Кондиционер включён.");
        public void TurnOff() => Console.WriteLine("Кондиционер выключен.");
        public void SetTemperature(double temperature) => Console.WriteLine($"Температура установлена на {temperature}°C.");
    }

    // Подсистема 3: Охранная система
    public class SecuritySystem
    {
        public void Activate() => Console.WriteLine("Охранная система активирована.");
        public void Deactivate() => Console.WriteLine("Охранная система деактивирована.");
    }

    class Program
    {
        static void Main()
        {
            // Создание объектов подсистем
            LightSystem lights = new LightSystem();
            AirConditioner ac = new AirConditioner();
            SecuritySystem security = new SecuritySystem();

            Console.WriteLine("Выберите режим работы умного дома (morning/night):");
            string mode = Console.ReadLine()?.ToLower();

            // Антипаттерн: логика управления распределена по клиентскому коду
            // При смене режима приходится изменять код в нескольких местах.
            if (mode == "morning")
            {
                // Режим \"Утро\":\n" + 
                // Освещение включается с яркостью 70%, кондиционер включается и устанавливается комфортная температура, охранная система отключается.
                lights.TurnOn();
                lights.SetBrightness(70);
                ac.TurnOn();
                ac.SetTemperature(22);
                security.Deactivate();
            }
            else if (mode == "night")
            {
                // Режим \"Ночь\":\n" + 
                // Освещение выключается, кондиционер выключается, охранная система включается.
                lights.TurnOff();
                ac.TurnOff();
                security.Activate();
            }
            else
            {
                Console.WriteLine("Неверно выбран режим.");
            }
        }
    }
}
