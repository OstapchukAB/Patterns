#region Паттерн "Шаблонный метод" (Template Method)
/*
Определение:
Шаблонный метод (Template Method) определяет общий алгоритм поведения подклассов,
позволяя им переопределить отдельные шаги этого алгоритма без изменения его структуры.

Цель: Определить скелет алгоритма в базовом классе, позволяя подклассам переопределять отдельные шаги без изменения структуры алгоритма.

Как работает:

Базовый класс объявляет шаблонный метод, который содержит последовательность шагов алгоритма.
Некоторые шаги реализуются в базовом классе (например, общие для всех подклассов),
а другие объявляются абстрактными или виртуальными, чтобы подклассы могли их переопределить.

 ***********
Когда использовать шаблонный метод?

Когда планируется, что в будущем подклассы должны будут переопределять различные этапы алгоритма без изменения его структуры
Когда в классах, реализующим схожий алгоритм, происходит дублирование кода.
Вынесение общего кода в шаблонный метод уменьшит его дублирование в подклассах.
 */
#endregion

namespace PatternTemplateMethod;
// Базовый класс с шаблонным методом - Производитель напитков
public abstract class BeverageMaker
{
    // Шаблонный метод, определяющий структуру алгоритма
    public void PrepareBeverage()
    {
        BoilWater(); //Вскипятить воду
        Brew(); //Заварить напиток
        PourInCup(); //Налить в чашку
        AddCondiments(); //Добавить добавки
    }

    // Общий шаг для всех напитков (Общие шаги, реализованные в базовом классе)
    private void BoilWater() => Console.WriteLine("Кипятим воду");
    private void PourInCup() => Console.WriteLine("Наливаем в чашку");

    // (Уникальные) шаги, которые должны реализовать подклассы 
    protected abstract void Brew();
    protected abstract void AddCondiments();
}

// Конкретные реализации
public class CoffeeMaker : BeverageMaker
{
    protected override void Brew() => Console.WriteLine("Завариваем кофе");
    protected override void AddCondiments() => Console.WriteLine("Добавляем молоко и сахар");
}

public class TeaMaker : BeverageMaker
{
    protected override void Brew() => Console.WriteLine("Завариваем чайные листья");
    protected override void AddCondiments() => Console.WriteLine("Добавляем лимон");
}
class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== Готовим кофе ===");
        BeverageMaker coffee = new CoffeeMaker();
        coffee.PrepareBeverage();

        Console.WriteLine("\n=== Готовим чай ===");
        BeverageMaker tea = new TeaMaker();
        tea.PrepareBeverage();
    }
}