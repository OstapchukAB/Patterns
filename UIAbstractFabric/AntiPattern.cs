#region Объяснение Антипаттерна
/*
    Антипаттерн:
    В данном решении фабрика реализована как один класс FurnitureFactory,
    который содержит условные операторы для создания конкретных объектов. 
    Если в будущем потребуется добавить новый стиль мебели, придется изменять этот класс,
    что нарушает принцип открытости/закрытости (OCP) и снижает расширяемость.

    Отсутствие семейства фабрик:
    Вместо того чтобы разделить производство грузовых и легковых автомобилей
    (или, в нашем случае, мебели для разных стилей) с помощью абстрактной фабрики,
    весь выбор завязан на простых if-else.

    Это решение иллюстрирует, как не следует организовывать создание семейств взаимосвязанных объектов,
    если планируется расширение функционала в будущем.
*/
#endregion
namespace FurnitureAntiPattern
{
    // Базовый класс для кресел
    public class Chair
    {
        public string Description { get; set; }
    }

    // Современное кресло
    public class ModernChair : Chair
    {
        public ModernChair()
        {
            Description = "Современное кресло с минималистичным дизайном";
        }
    }

    // Классическое кресло
    public class ClassicChair : Chair
    {
        public ClassicChair()
        {
            Description = "Классическое кресло с изысканной резьбой";
        }
    }

    // Базовый класс для столов
    public class Table
    {
        public string Description { get; set; }
    }

    // Современный стол
    public class ModernTable : Table
    {
        public ModernTable()
        {
            Description = "Современный стол из стекла и металла";
        }
    }

    // Классический стол
    public class ClassicTable : Table
    {
        public ClassicTable()
        {
            Description = "Классический стол из дерева с резными деталями";
        }
    }

    // Фабрика для создания мебели (антипаттерн: нет разделения семейств, жесткие if-else)
    public class FurnitureFactory
    {
        public Chair CreateChair(string style)
        {
            if (style == "modern")
            {
                return new ModernChair();
            }
            else if (style == "classic")
            {
                return new ClassicChair();
            }
            else
            {
                throw new ArgumentException("Неподдерживаемый стиль мебели");
            }
        }

        public Table CreateTable(string style)
        {
            if (style == "modern")
            {
                return new ModernTable();
            }
            else if (style == "classic")
            {
                return new ClassicTable();
            }
            else
            {
                throw new ArgumentException("Неподдерживаемый стиль мебели");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Выберите стиль мебели (modern/classic):");
            string style = Console.ReadLine()?.ToLower();

            // Используем один класс фабрики с условными операторами (антипаттерн)
            FurnitureFactory factory = new FurnitureFactory();

            try
            {
                Chair chair = factory.CreateChair(style);
                Table table = factory.CreateTable(style);

                Console.WriteLine("Кресло: " + chair.Description);
                Console.WriteLine("Стол: " + table.Description);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
