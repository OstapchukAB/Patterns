/*
 Краткое пояснение:

    NotificationService:
    Метод SendNotification использует условные операторы для определения типа уведомления и создания соответствующего объекта.
При добавлении нового типа уведомления (например, InAppNotification) потребуется изменить этот метод, что затрудняет масштабирование и сопровождение кода.

Этот код является отправной точкой для практики применения паттерна «Фабричный метод»,
который позволит вынести логику создания объектов в отдельные классы (фабрики) и устранить множественные условия в NotificationService.
 */
namespace NotificationSystem
{
    // Базовый класс для уведомлений
    public abstract class Notification
    {
        protected readonly string _message;

        public Notification(string message)
        {
            _message = message;
        }

        // Метод для отправки уведомления
        public virtual void Notify()
        {
            Console.WriteLine("Notification: " + _message);

        }

    }

    public abstract class FactoryNotification
    {
        public abstract Notification FabricMethodNotification();
    }

    public class CreateEmailNotification : FactoryNotification
    {
        readonly string _message;
        public CreateEmailNotification(string message)
        {
            _message = message;
        }

        public override Notification FabricMethodNotification()
        {
            return new EmailNotification(_message);
        }
    }
    public class CreateSmsNotification : FactoryNotification
    {
        readonly string _message;
        public CreateSmsNotification(string message)
        {
            _message = message;
        }
        public override Notification FabricMethodNotification()
        {
            return new SmsNotification(_message);
        }
    }

    public class CreatePushNotification : FactoryNotification
    {
        readonly string _message;
        public CreatePushNotification(string message)
        {
            _message = message;
        }
        public override Notification FabricMethodNotification()
        {
            return new PushNotification(_message);
        }
    }

    // Конкретное уведомление: Email
    class EmailNotification : Notification
    {
        public EmailNotification(string message) : base(message) { }

        public override void Notify()
        {
            Console.WriteLine("Email sent: " + _message);
        }
    }

    // Конкретное уведомление: SMS
    class SmsNotification : Notification
    {
        public SmsNotification(string message) : base(message) { }

        public override void Notify()
        {
            Console.WriteLine("SMS sent: " + _message);
        }
    }

    // Конкретное уведомление: Push
    class PushNotification : Notification
    {
        public PushNotification(string message) : base(message) { }

        public override void Notify()
        {
            Console.WriteLine("Push notification sent: " + _message);
        }
    }

    // Сервис уведомлений 
    public class NotificationService
    {
        public void SendNotification(FactoryNotification factoryNotification)
        {
            factoryNotification.FabricMethodNotification().Notify();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Пример использования сервиса уведомлений
            NotificationService service = new ();

            // Отправляем Email-уведомление
            service.SendNotification(new CreateEmailNotification("Your order has been shipped."));

            // Отправляем SMS-уведомление
            service.SendNotification(new CreateSmsNotification("Your verification code is 1234."));

            // Отправляем Push-уведомление
            service.SendNotification(new CreatePushNotification("You have a new friend request."));
        }
    }
}
