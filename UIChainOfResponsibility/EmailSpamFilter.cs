using System;

namespace UIChainOfResponsibilitySpamFilter;

public class  Email
{
    public Email(string sender, string text)
    {
        Sender = sender;
        Text = text;
    }
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
                Console.WriteLine($"Письмо {email.Sender} содержит спам");
                email.PathEmail = EmailType.SPAM;
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
                Console.WriteLine($"Письмо {email.Sender} от важного отправителя");
                email.PathEmail = EmailType.IMPORTANT;
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
                Console.WriteLine($"Письмо {email.Sender} от коллеги");
                email.PathEmail = EmailType.COWORKER;
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
                Console.WriteLine($"Письмо {email.Sender} содержит ключевые слова");
                email.PathEmail = EmailType.KEYSWORD;
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
        Console.WriteLine($"Письмо {email.Sender} обычное");
        email.PathEmail = EmailType.DEFAULT;
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
            new Email("user@gmail.com", "распродажа"),
            new Email("director@copr.com", "выполнить распоряжение"),
            new Email("coworker@corp.com", "направляю вам служебное поручение"),
            new Email("user4", "привет"),
            new Email("user", "пароль"),
        };

        foreach (var item in emails)
        {
            spam.HandleRequest(item);

            Console.WriteLine($"письмо {item.Sender}  помечено:[{item.PathEmail}]");
        }

    }
}