using System;
/*
 Сохранили каноническое применение "Фабричного метода"

    Вместо интерфейса INotificationFactory используется абстрактный класс NotificationFactory.
    Теперь общее поведение (SendNotification) находится в базовом классе, а конкретные фабрики переопределяют CreateNotification().

Меньше кода в Program

    Теперь не нужно передавать фабрику в NotificationService, а затем вручную вызывать Notify().
    Достаточно вызвать SendNotification() у фабрики, и всё происходит автоматически.

Инкапсуляция логики в фабриках

    Логика создания объекта скрыта в фабрике.
    SendNotification() теперь вызывает Notify() у созданного объекта.

Теперь код чётко следует паттерну "Фабричный метод", без избыточности интерфейсов. 
 */
namespace NotificationSystem
{
    // Базовый класс для уведомлений
    public abstract class Notification
    {
        protected readonly string _message;

        protected Notification(string message)
        {
            _message = message;
        }

        // Метод для отправки уведомления
        public abstract void Notify();
    }

    // Конкретные классы уведомлений
    public class EmailNotification : Notification
    {
        private readonly string _subject;

        public EmailNotification(string message, string subject) : base(message)
        {
            _subject = subject;
        }

        public override void Notify()
        {
            Console.WriteLine($"Email sent: Subject: {_subject}, Message: {_message}");
        }
    }

    public class SmsNotification : Notification
    {
        private readonly string _sender;

        public SmsNotification(string message, string sender) : base(message)
        {
            _sender = sender;
        }

        public override void Notify()
        {
            Console.WriteLine($"SMS sent from {_sender}: {_message}");
        }
    }

    public class PushNotification : Notification
    {
        private readonly int _priority;

        public PushNotification(string message, int priority) : base(message)
        {
            _priority = priority;
        }

        public override void Notify()
        {
            Console.WriteLine($"Push notification sent [Priority: {_priority}]: {_message}");
        }
    }

    // Абстрактный класс фабрики
    public abstract class NotificationFactory
    {
        public abstract Notification CreateNotification(string message);

        public void SendNotification(string message)
        {
            var notification = CreateNotification(message);
            notification.Notify();
        }
    }

    // Фабрика Email-уведомлений
    public class EmailNotificationFactory : NotificationFactory
    {
        public override Notification CreateNotification(string message)
        {
            return new EmailNotification(message, "Important Notice");
        }
    }

    // Фабрика SMS-уведомлений
    public class SmsNotificationFactory : NotificationFactory
    {
        public override Notification CreateNotification(string message)
        {
            return new SmsNotification(message, "Service123");
        }
    }

    // Фабрика Push-уведомлений
    public class PushNotificationFactory : NotificationFactory
    {
        public override Notification CreateNotification(string message)
        {
            return new PushNotification(message, priority: 1);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Создаём фабрики
            NotificationFactory emailFactory = new EmailNotificationFactory();
            NotificationFactory smsFactory = new SmsNotificationFactory();
            NotificationFactory pushFactory = new PushNotificationFactory();

            // Используем метод SendNotification(), который инкапсулирует создание и отправку уведомления
            emailFactory.SendNotification("Your order has been shipped.");
            smsFactory.SendNotification("Your verification code is 1234.");
            pushFactory.SendNotification("You have a new friend request.");
        }
    }
}
