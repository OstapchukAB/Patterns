#region Задание - Система уведомлений с динамическим расширением функциональности
/*
Требования:

Базовая функциональность:
   Создайть базовый интерфейс INotification с методом Send(), который отправляет уведомление (например, выводит сообщение в консоль).
   Реализовать класс BasicNotification, который отправляет простое уведомление.

Декораторы:
   Создайте абстрактный класс NotificationDecorator, реализующий INotification и принимающий INotification в конструкторе.
   Реализуйте конкретные декораторы, добавляющие дополнительное поведение:
    - SMSDecorator:   Дополнительное отправление уведомления по SMS (вывод строки, что SMS отправлено).
    - EmailDecorator: Дополнительное отправление уведомления по Email.
    - PushDecorator:  Дополнительное отправление push-уведомления.

Клиентский код:
    - В методе Main создайте объект BasicNotification.
    - Затем оберните его в один или несколько декораторов, чтобы добавить функциональность (например, отправку по SMS и Email).
    - Вызовите метод Send(), и убедитесь, что все декораторы отрабатывают своё поведение.

Задача: 
  Реализовать данное задание, применяя паттерн «Декоратор». 
  Постараться добиться, чтобы базовое уведомление могло быть расширено дополнительными способами динамически.
 */
#endregion

namespace NotificationPatternDecorator;

public interface INotification
{
    void Send(string message);
}

class BaseNotification : INotification
{
    public void Send(string message)
    {
        Console.WriteLine("Отправка  уведомления: " + message);
    }
}

public abstract class NotificationDecorator:INotification
{
    private readonly INotification _notification;   
        
    public NotificationDecorator(INotification notification)
    {
        _notification = notification;
    }

    public virtual void Send(string message)
    {
        _notification.Send(message);
    }
}
public class SmsDecorator : NotificationDecorator
{
    public SmsDecorator(INotification notification) : base(notification)
    {
    }
    public override void Send(string message)
    {
        base.Send(message);
        Console.WriteLine($"Отправка SMS:{message}");
    }
}
public class EmmailDecorator : NotificationDecorator
{
    public EmmailDecorator(INotification notification) : base(notification)
    {
    }
    public override void Send(string message)
    {
        base.Send(message);
        Console.WriteLine($"Отправка Email:{message}");
    }
}

public class PushDecorator : NotificationDecorator
{
    public PushDecorator(INotification notification) : base(notification)
    {
    }
    public override void Send(string message)
    {
        base.Send(message);
        Console.WriteLine($"Отправка Push:{message}");
    }
}

class Program
{
    static void Main()
    {
        INotification notification= new BaseNotification();
        var message = "Важное сообщение";
       // notify.Send(message);
        
        notification= new SmsDecorator(notification);

        notification = new EmmailDecorator(notification);

        notification = new PushDecorator(notification);
        notification.Send(message);
    }
}
