using AbstractFactoryExample;

//class Program
//{
//    static void Main(string[] args)
//    {
//        // Жёсткая привязка к конкретным реализациям (Windows)
//        WinButton button = new WinButton();
//        WinCheckbox checkbox = new WinCheckbox();

//        button.Paint();
//        checkbox.Paint();

//        Console.ReadKey();
//    }
//}
class Program
{
     void Main(string[] args)
    {
        // Клиентский код работает с абстракцией фабрики
        IGUIFactory guiFactory = new WindowsFactory();

        IButton button = guiFactory.CreateButton();
        ICheckbox checkbox = guiFactory.CreateCheckbox();

        button.Paint();
        checkbox.Paint();

        Console.ReadKey();
    }
}
