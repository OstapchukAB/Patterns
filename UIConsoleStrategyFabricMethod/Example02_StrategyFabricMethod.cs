namespace TextTransformationAntiPattern;
#region Начальные условия
/*
Тема: Преобразование текста

Исходные требования (антипаттерн):

    Программа читает строку от пользователя.
    Программа предлагает выбрать тип преобразования:
        Преобразовать в верхний регистр.
        Преобразовать в нижний регистр.
        Реверсировать строку.
    На основании выбранного типа преобразования выполняется нужная операция с помощью условных операторов внутри одного метода.

Задача:

    Рефакторинг: Перепиши исходный код, используя паттерны:
        Стратегия: Для каждого типа преобразования создай отдельную стратегию (класс), реализующую общий интерфейс.
        Фабричный метод: Создай фабрику (либо абстрактного создателя, либо простую статическую фабрику),
которая будет возвращать нужную стратегию на основании выбранного типа преобразования.

    Контекст: Реализуй класс-контекст, который будет принимать стратегию и делегировать ей выполнение преобразования текста.

    Демонстрация работы: В методе Main запроси у пользователя строку и тип преобразования, 
затем используй фабрику для создания стратегии, передай её в контекст и выведи результат.
 */
#endregion
#region Что нужно сделать
/*
 Твоя задача:

    Реализуй интерфейс стратегии (например, ITextTransformationStrategy) с методом для преобразования текста.

    Создай конкретные стратегии:
        UpperCaseStrategy – преобразует строку в верхний регистр.
        LowerCaseStrategy – преобразует строку в нижний регистр.
        ReverseStringStrategy – переворачивает строку (реверс).

    Реализуй фабрику (например, класс TextTransformationStrategyFactory),
которая по входящему типу (например, строке "upper", "lower", "reverse") возвращает нужную стратегию.

    Создай класс-контекст (например, TextTransformer), который принимает стратегию (через конструктор или метод) и делегирует ей выполнение преобразования.

    В методе Main:
        Запроси у пользователя исходную строку и тип преобразования.
        Используй фабрику для создания соответствующей стратегии.
        Передай стратегию в контекст и получи преобразованный текст.
        Выведи результат в консоль.
 */
#endregion
#region Стратегии обработки строки
public interface ITextTransformStrategy
{
	public string TransformText(string text);
}
public class TextToUpperStrategy:ITextTransformStrategy
{
	public string TransformText(string text)=>text.ToUpper();
}
public class TextToLowerStrategy:ITextTransformStrategy
{
	public string TransformText(string text)=>text.ToLower();
}
public class TextToReverseStrategy:ITextTransformStrategy
{
	public string TransformText(string text)
	{
		 char[] arr = text.ToCharArray();
         Array.Reverse(arr);
         return new string(arr);
	}
}

public class TextContext
{
    ITextTransformStrategy _strategy;
	public TextContext(ITextTransformStrategy strategy)
	{
		_strategy=strategy;
	}
	public void SetStrategy(ITextTransformStrategy strategy)
	{
		_strategy=strategy;
	}
	public string ExecuteAlgoritm(string text)
	{
		return _strategy.TransformText(text);
	}
}
#endregion

#region Фабрика стратегий
abstract class TextStrategy
{
    public abstract ITextTransformStrategy GetStrategy();
}
class TextToUpper : TextStrategy
{
    public override ITextTransformStrategy GetStrategy()=> new TextToUpperStrategy();
}
class TextToLower : TextStrategy
{
    public override ITextTransformStrategy GetStrategy()=> new TextToLowerStrategy();

}
class TextToReverse : TextStrategy
{
    public override ITextTransformStrategy GetStrategy()=> new TextToReverseStrategy();
}
abstract class CreatorStrategy
{
	public abstract TextStrategy FactoryMethod();
}
class CreatorTextToUpper : CreatorStrategy
{
    public override TextStrategy FactoryMethod() => new TextToUpper();
}
class CreatorTextToLower :CreatorStrategy
{
    public override TextStrategy FactoryMethod() => new TextToLower();
}
class CreatorTextToReverse :CreatorStrategy
{
	public override TextStrategy FactoryMethod()=>new TextToReverse();
}
#endregion

class Program
{
     static void Main(string[] args)
    {
        Console.WriteLine("Введите строку для преобразования:");
        string input = Console.ReadLine();

        Console.WriteLine("Выберите тип преобразования (upper, lower, reverse):");
        string type = Console.ReadLine();
		
		
        TextStrategy? factory = type switch 
        { 
            "upper" => new CreatorTextToUpper().FactoryMethod(),
            "lower" => new CreatorTextToLower().FactoryMethod(),
            "reverse" => new CreatorTextToReverse().FactoryMethod(),
            _=> null
        };
        if (factory == null)
        {
            Console.WriteLine("данная стратегия не рализована!");
            return;
        }
        var strategy = factory.GetStrategy();
        var textContext= new TextContext(strategy);
        Console.WriteLine("Результат: " + textContext.ExecuteAlgoritm(input));
    }
}
