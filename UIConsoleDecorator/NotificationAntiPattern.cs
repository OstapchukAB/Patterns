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

namespace NotificationAntiPattern
{
    // Класс, который объединяет всю логику отправки уведомлений
    public class Notification
    {
        public string Message { get; set; }
        public bool SendSMS { get; set; }
        public bool SendEmail { get; set; }
        public bool SendPush { get; set; }

        public Notification(string message, bool sendSMS, bool sendEmail, bool sendPush)
        {
            Message = message;
            SendSMS = sendSMS;
            SendEmail = sendEmail;
            SendPush = sendPush;
        }

        // Метод, который отправляет уведомление по разным каналам в зависимости от настроек
        public void Send()
        {
            Console.WriteLine("Отправка базового уведомления: " + Message);
            if (SendSMS)
            {
                // Логика отправки SMS
                Console.WriteLine("Отправлено SMS уведомление: " + Message);
            }
            if (SendEmail)
            {
                // Логика отправки Email
                Console.WriteLine("Отправлено Email уведомление: " + Message);
            }
            if (SendPush)
            {
                // Логика отправки Push уведомления
                Console.WriteLine("Отправлено Push уведомление: " + Message);
            }
        }
    }

    class Program
    {
        static void Main()
        {
            // Пример использования: уведомление отправляется по всем каналам
            Notification notification = new Notification("Важное уведомление!", true, true, true);
            notification.Send();
        }
    }
}
