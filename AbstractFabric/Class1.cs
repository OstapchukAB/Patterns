using System;

namespace WithoutAbstractFactory
{
    // Класс, отвечающий за создание кнопки для Windows
    public class WinButton
    {
        public void Paint()
        {
            Console.WriteLine("Отрисовка кнопки Windows.");
        }
    }

    // Класс, отвечающий за создание флажка для Windows
    public class WinCheckbox
    {
        public void Paint()
        {
            Console.WriteLine("Отрисовка флажка Windows.");
        }
    }


}


namespace AbstractFactoryExample
{
    // Абстракции для компонентов
    public interface IButton
    {
        void Paint();
    }

    public interface ICheckbox
    {
        void Paint();
    }

    // Абстрактная фабрика для создания элементов графического интерфейса
    public interface IGUIFactory
    {
        IButton CreateButton();
        ICheckbox CreateCheckbox();
    }

    // Конкретные реализации для Windows
    public class WindowsButton : IButton
    {
        public void Paint()
        {
            Console.WriteLine("Отрисовка кнопки Windows (Abstract Factory).");
        }
    }

    public class WindowsCheckbox : ICheckbox
    {
        public void Paint()
        {
            Console.WriteLine("Отрисовка флажка Windows (Abstract Factory).");
        }
    }

    // Конкретная фабрика для Windows
    public class WindowsFactory : IGUIFactory
    {
        public IButton CreateButton()
        {
            return new WindowsButton();
        }

        public ICheckbox CreateCheckbox()
        {
            return new WindowsCheckbox();
        }
    }

   
}

