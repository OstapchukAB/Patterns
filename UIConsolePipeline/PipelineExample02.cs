#region Описание нижеуказанной реализации
/*
 Данный вариант реализации паттерна "Конвейер" отлично подходит для сценариев, где все шаги работают с одним типом данных (в данном случае string).
 Он проще и безопаснее канонического примера за счет отказа от рефлексии, но имеет некоторые особенности. Разберём плюсы, минусы и отличия.

Интерфейс IPipelineStep<T>:
    Каждый шаг принимает и возвращает данные типа T.
    Упрощает реализацию, но ограничивает конвейер работой с одним типом.
********************************
Преимущества данной реализации
    Безопасность типов:
        Нет рефлексии (Invoke), все шаги вызываются напрямую. Это исключает ошибки времени выполнения, связанные с несовпадением типов.
    Простота:
        Код легче читать и поддерживать, так как отсутствуют сложные обобщения с TInput и TOutput.
    Эффективность:
        Отказ от рефлексии повышает производительность.
    Fluent-интерфейс:
        Цепочка вызовов .AddStep(...).AddStep(...) делает код наглядным.
******************************
Недостатки и ограничения
    Фиксированный тип данных:
        Все шаги должны работать с одним типом (T). Например, нельзя добавить шаг, преобразующий string в int.
    Мутабельность конвейера:
        Метод AddStep изменяет текущий экземпляр конвейера. Если где-то сохранить промежуточный конвейер, это может привести к неожиданному поведению.
   Меньшая гибкость:
        Нельзя динамически менять типы данных между шагами, как в канонической реализации.
*******************************
Когда использовать этот ваш вариант
    Если все шаги конвейера работают с одним типом данных (например, фильтрация/преобразование строк, коллекций, чисел).
    Когда важна простота и безопасность типов.
    Для задач, где не требуется менять тип данных между этапами.
Примеры:
    Обработка текста (очистка, форматирование, маскирование).
    Преобразование коллекций (фильтрация → сортировка → агрегация).
    Построение цепочек валидации.
 */
#endregion

namespace PipelineExample
{
    // Интерфейс для шага конвейера
    public interface IPipelineStep<T>
    {
        T Process(T input);
    }

    // Первый шаг: преобразование строки в верхний регистр
    public class UpperCaseStep : IPipelineStep<string>
    {
        public string Process(string input)
        {
            return input.ToUpper();
        }
    }

    // Второй шаг: реверс строки
    public class ReverseStep : IPipelineStep<string>
    {
        public string Process(string input)
        {
            char[] charArray = input.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
    }

    // Третий шаг: добавление суффикса
    public class AppendSuffixStep : IPipelineStep<string>
    {
        public string Process(string input)
        {
            return input + " - Processed";
        }
    }

    // Класс конвейера, который последовательно выполняет все этапы
    public class Pipeline<T>
    {
        private readonly List<IPipelineStep<T>> _steps = new List<IPipelineStep<T>>();

        // Добавление шага в конвейер
        public Pipeline<T> AddStep(IPipelineStep<T> step)
        {
            _steps.Add(step);
            return this;
        }

        // Выполнение конвейера
        public T Execute(T input)
        {
            T result = input;
            foreach (var step in _steps)
            {
                result = step.Process(result);
            }
            return result;
        }
    }

    class Program
    {
        void Main(string[] args)
        {
            // Создаём конвейер и добавляем в него шаги
            var pipeline = new Pipeline<string>();
            pipeline.AddStep(new UpperCaseStep())
                    .AddStep(new ReverseStep())
                    .AddStep(new AppendSuffixStep());

            // Входные данные
            string input = "Hello, Pipeline!";
            // Выполняем конвейер
            string result = pipeline.Execute(input);

            Console.WriteLine("Результат: " + result);
        }
    }
}
