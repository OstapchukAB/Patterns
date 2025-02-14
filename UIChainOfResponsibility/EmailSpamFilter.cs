using System;

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