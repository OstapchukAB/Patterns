using System;
#region  Задание для  реализации
/*
Тема: Реализация системы фильтрации входящих писем с помощью паттерна «Цепочка обязанностей»

Требования:

    Описание задачи:
    Представьте, что вам нужно реализовать систему обработки входящих электронных писем. 
Каждое письмо проходит через цепочку фильтров, которые могут решать, что делать с письмом:
        Антиспам-фильтр: Если письмо содержит спам-фразы, оно помечается как спам.
        Фильтр по важности: Если письмо помечено как важное (например, от определённого отправителя), оно помечается как важное.
        Фильтр по содержимому: Если письмо содержит определённые ключевые слова, оно перемещается в соответствующую папку.

    Требования к реализации:
        Создайте абстрактный класс (например, EmailFilter) с методом HandleEmail(Email email).
        Реализуйте несколько конкретных фильтров, например:
            SpamFilter – проверяет письмо на наличие спам-фраз.
            PriorityFilter – проверяет отправителя или тему письма.
            ContentFilter – проверяет содержание письма на ключевые слова.
        Свяжите фильтры в цепочку так, чтобы письмо проходило через все фильтры до обработки.
        Если письмо обработано (например, помечено как спам или перемещено в папку), 
фильтры могут завершить цепочку (либо продолжить, если требуется многократная обработка).
        Определите класс Email с нужными свойствами (например, Sender, Subject, Content, IsSpam, IsPriority, Folder).

    Задание:
        Реализуйте классы Email, EmailFilter (абстрактный), а также конкретные фильтры: SpamFilter, PriorityFilter и ContentFilter.
        Создайте цепочку фильтров и протестируйте её на примере входящего письма, выводя итоговое состояние письма на экран.
 
 */
#endregion
#region Структура проекта
/*
 EmailProcessingSystem/
│── EmailProcessingSystem.sln   # Файл решения
│
├── EmailProcessingSystem/       # Основной проект
│   ├── Program.cs               # Точка входа
│   ├── Models/
│   │   ├── Email.cs             # Модель письма
│   │   ├── EmailType.cs         # Перечисление типов писем
│   ├── Handlers/                # Обработчики фильтров
│   │   ├── EmailFilterHandler.cs  # Базовый класс обработчика
│   │   ├── HandlerSpam.cs         # Фильтр спама
│   │   ├── HandlerImportant.cs    # Фильтр важных писем
│   │   ├── HandlerCoworker.cs     # Фильтр писем от коллег
│   │   ├── HandlerKeysWord.cs     # Фильтр по ключевым словам
│   │   ├── HandlerDefault.cs      # Обработчик по умолчанию
│   ├── Services/
│   │   ├── EmailSorter.cs       # Класс сортировки писем по папкам
│
├── EmailProcessingSystem.Tests/  # Тесты
│   ├── EmailFilterTests.cs       # Тесты обработки писем
│   ├── EmailSorterTests.cs       # Тесты сортировки писем
│
└── README.md                     # Документация проекта
Объяснение структуры:

 Models/ – хранит основные модели, такие как Email и EmailType.
 Handlers/ – содержит все классы фильтрации (по спаму, важности, ключевым словам и т. д.).
 Services/ – хранит EmailSorter, который отвечает за распределение писем по папкам.
 Tests/ – тестирует обработку писем и их сортировку.
 */
#endregion
namespace UIChainOfResponsibilitySpamFilter;
#region классы модели и вспомогательные
public class  Email
{
    public Email(int id,string sender, string text)
    {
        Id= id;
        Sender = sender;
        Text = text;
    }
    public int Id;
    public string Sender { get; }
    public string Text { get; }
    public EmailType PathEmail { get;  set; } = EmailType.DEFAULT;

}

public enum EmailType
{
    SPAM,
    IMPORTANT,
    COWORKER,
    KEYSWORD,
    DEFAULT
}

public class EmailSorter
{
    private readonly Dictionary<EmailType, string> _folders = new()
    {
        { EmailType.SPAM, "Spam" },
        { EmailType.IMPORTANT, "Important" },
        { EmailType.COWORKER, "Coworkers" },
        { EmailType.KEYSWORD, "Keywords" },
        { EmailType.DEFAULT, "Inbox" }
    };

    public void SortEmail(Email email)
    {
        string folder = _folders[email.PathEmail];
        Console.WriteLine($"Письмо № [{email.Id}] от {email.Sender} перемещено в папку [{folder}].");
    }
}

#endregion
public abstract  class EmailFilterHandler
{
   
    public EmailFilterHandler? Successor { get; set; }
    public abstract void HandleRequest(Email email);
    
}
//Антиспам-фильтр
class HandlerSpam : EmailFilterHandler
{
    string[] spam = ["распродажа","бесплатно"];
    public override void HandleRequest(Email email)
    {

        foreach (var item in spam)
        {
            if (email.Text.Contains(item))
            {
                email.PathEmail = EmailType.SPAM;
                Console.WriteLine($"письмо {email.Sender}  помечено:[{email.PathEmail}]");
                return;
            }
        }

        if (Successor != null)
        {
            Successor.HandleRequest(email);
        }
    }
}
class HandlerImportant : EmailFilterHandler
{
    string[] sender = ["director", "boss"];   
    public override void HandleRequest(Email email)
    {
        foreach (var item in sender)
        {
            if (email.Sender.Contains(item))
            {
                email.PathEmail = EmailType.IMPORTANT;
                Console.WriteLine($"письмо {email.Sender}  помечено:[{email.PathEmail}]");
                return;
            }
        }
        if (Successor != null)
        {
            Successor.HandleRequest(email);
        }
    }
}

class HandlerCoworker : EmailFilterHandler
{
    string[] sender = ["@corp.com"];
    public override void HandleRequest(Email email)
    {
        foreach (var item in sender)
        {
            if (email.Sender.Contains(item))
            {
                email.PathEmail = EmailType.COWORKER;
                Console.WriteLine($"письмо  {email.Sender}  помечено:[{email.PathEmail}]");
                return;
            }
        }
        if (Successor != null)
        {
            Successor.HandleRequest(email);
        }
    }
}

class HandlerKeysWord : EmailFilterHandler
{
    string[] keys = ["деньги", "пароль", "секрет"];
    public override void HandleRequest(Email email)
    {
        foreach (var item in keys)
        {
            if (email.Text.Contains(item))
            {
                email.PathEmail = EmailType.KEYSWORD;
                Console.WriteLine($"письмо {email.Sender}  помечено:[{email.PathEmail}]");
                return;
            }
        }
        if (Successor != null)
        {
            Successor.HandleRequest(email);
        }
    }
}

class HandlerDefault : EmailFilterHandler
{
    public override void HandleRequest(Email email)
    {
        email.PathEmail = EmailType.DEFAULT;
        Console.WriteLine($"письмо {email.Sender}  помечено:[{email.PathEmail}]");
    }
}

public class Program
{
    public static void Main()
    {
        EmailFilterHandler spam = new HandlerSpam();
        EmailFilterHandler important = new HandlerImportant();
        EmailFilterHandler coworker = new HandlerCoworker();
        EmailFilterHandler keys = new HandlerKeysWord();
        EmailFilterHandler def = new HandlerDefault();

        spam.Successor = important;
        important.Successor = coworker;
        coworker.Successor = keys;
        keys.Successor = def;

        var emails = new Email[]
        {
            new Email(111,"user@gmail.com", "распродажа"),
            new Email(120,"director@copr.com", "выполнить распоряжение"),
            new Email(1234,"coworker@corp.com", "направляю вам служебное поручение"),
            new Email(1999,"user4", "привет"),
            new Email(2456,"user", "пароль"),
        };

        var sorter = new EmailSorter();

        foreach (var item in emails)
        {
            //начинаем обработку с данного уровня
            spam.HandleRequest(item);

            sorter.SortEmail(item);    // Перемещение в нужную папку
        }

    }
}