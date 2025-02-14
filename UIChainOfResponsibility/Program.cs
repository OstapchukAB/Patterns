#region Паттерн "Цепочка обязанностей" (Chain of Responsibility)
/*
 Для чего нужен?
Паттерн позволяет передавать запросы последовательно по цепочке обработчиков. 
Каждый обработчик решает, может ли он обработать запрос, или передает его следующему в цепочке. 
Это помогает:
   Уменьшить связанность между отправителем запроса и получателем.
   Динамически настраивать порядок обработки.
   Добавлять новые обработчики без изменения существующего кода.

Свойства:
    Обработчики связаны в цепочку.
    Запрос проходит по цепочке, пока не будет обработан.
    Клиент не знает, какой именно обработчик выполнит запрос.

Когда применять?
    Когда есть несколько объектов, способных обработать запрос, и порядок их вызова не фиксирован.
    Когда обработчики должны определяться динамически (например, уровни доступа, фильтры).
    Примеры: системы логирования, валидации данных, обработки HTTP-запросов в middleware.
 */
#endregion
#region  Пример на C#
/*
Сценарий: Система одобрения заявок на покупку с разными лимитами:
    Менеджер: До 1000 дол
    Директор: До 5000 долл
    Генеральный директор: Любая сумма.
 */
#endregion
#region  Как это работает?
/*
    Клиент инициирует запрос через первый обработчик (Manager).
    Если обработчик не может выполнить запрос, он передает его следующему в цепочке.
    Процесс повторяется, пока запрос не будет обработан или цепочка не закончится.

Преимущества:
    Гибкость: Можно менять порядок обработчиков или добавлять новые.
    Отсутствие жестких связей: Отправитель запроса не зависит от конкретных обработчиков.
 */
#endregion
namespace Chain_of_Responsibility;
// Абстрактный обработчик
public abstract class Approver
{
    //утверждающий преемник
    Approver? _successor;
    protected Approver? Successor 
    {
        get 
        {
            return _successor;
        }
        set 
        {
           
            _successor = value;
            Console.WriteLine($"Successor {_successor}");
        }
    }

    public void SetSuccessor(Approver successor)
    {
        this.Successor = successor;
    }

    public abstract void HandleRequest(int amount);
}

// Конкретные обработчики
public class Manager : Approver
{
    public override void HandleRequest(int amount)
    {
        if (amount <= 1000)
        {
            Console.WriteLine($"Менеджер одобрил заявку на {amount}");
        }
        else if (Successor != null)
        {
            Successor.HandleRequest(amount);
        }
    }
}

public class Director : Approver
{
    public override void HandleRequest(int amount)
    {
        if (amount <= 5000)
        {
            Console.WriteLine($"Директор одобрил заявку на {amount}");
        }
        else if (Successor != null)
        {
            Successor.HandleRequest(amount);
        }
    }
}

public class CEO : Approver
{
    public override void HandleRequest(int amount)
    {
        Console.WriteLine($"Генеральный директор одобрил заявку на {amount}");
    }
}

// Клиентский код
class Program
{
     void Main()
    {
        // Настройка цепочки
        Approver manager = new Manager();
        Approver director = new Director();
        Approver ceo = new CEO();

        //здесь устанавливаем цепочку
        manager.SetSuccessor(director);
        director.SetSuccessor(ceo);

        // Тестирование
        var dollars= new int[] { 800, 3000, 10000 };

        foreach (int i in dollars) 
        { 
            manager.HandleRequest(i);
        }
        //manager.HandleRequest(800);   // Менеджер
        //manager.HandleRequest(3000);  // Директор
        //manager.HandleRequest(10000); // Генеральный директор
    }
}