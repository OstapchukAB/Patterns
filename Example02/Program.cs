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
    abstract class Notification
    {
        public readonly string _message;

        public Notification(string message)
        {
            _message = message;
        }

        // Метод для отправки уведомления
        public virtual void Notify()
        {
            Console.WriteLine("Notification: " + _message);

        }
        //фабричный метод
        public abstract Notification FactoryNotification();
    }

    // Конкретное уведомление: Email
    class EmailNotification : Notification
    {
        public EmailNotification(string message) : base(message) { }

        public override Notification FactoryNotification()
        {
            return new EmailNotification(_message);
        }

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
        public override Notification FactoryNotification()
        {
            return new SmsNotification(_message);
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
        public override Notification FactoryNotification()
        {
            return new PushNotification(_message);
        }
    }

    // Сервис уведомлений 
    class NotificationService
    {
        public void SendNotification(Notification notification)
        {
            notification.FactoryNotification().Notify();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Пример использования сервиса уведомлений
            NotificationService service = new NotificationService();

            // Отправляем Email-уведомление
            service.SendNotification(new EmailNotification("Your order has been shipped."));

            // Отправляем SMS-уведомление
            service.SendNotification(new SmsNotification("Your verification code is 1234."));

            // Отправляем Push-уведомление
            service.SendNotification(new PushNotification("You have a new friend request."));
        }
    }
}
