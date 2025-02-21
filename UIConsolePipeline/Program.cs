#region Паттерн "Конвейер" (Pipeline) 
/*
Конвеер - это архитектурный паттерн, который организует обработку данных или запросов в виде последовательности шагов (этапов),
где каждый шаг выполняет определенную операцию и передает результат следующему шагу.
Этот подход напоминает конвейер на производстве, где продукт проходит через несколько стадий обработки.

Когда используется?
- Многоэтапная обработка данных: Например, фильтрация, преобразование, валидация.
- Разделение ответственности: Каждый шаг решает одну задачу, упрощая код.
- Гибкость: Шаги можно переставлять, добавлять или удалять без изменения основной логики.
- Параллельная обработка: Отдельные этапы могут выполняться асинхронно или в параллельных потоках.
 */
#endregion

#region Канонический пример применения Паттерна -конвейер для обработки текста
/*
 Реализуем конвейер для обработки текста:
    Удаление пробелов.
    Приведение к верхнему регистру.
    Добавление префикса.
 */
#endregion
#region  Преимущества нижеописанного кода с применением Паттерна
/*
 Преимущества
    Модульность: Каждый шаг можно тестировать отдельно.
    Гибкость: Порядок шагов легко изменить.
    Расширяемость: Новые шаги добавляются без изменения существующего кода.
Этот паттерн часто применяется в библиотеках для обработки HTTP-запросов (ASP.NET Core Middleware),
ETL-процессах или сложных преобразованиях данных.
 */
#endregion
namespace PatternPipeLine
{
    //1. Интерфейс шага конвейера
    public interface IPipelineStep<TInput, TOutput>
    {
        TOutput Process(TInput input);
    }

    //2. Конкретные шаги
    //Шаг 1: Удаление пробелов
    public class RemoveWhitespaceStep : IPipelineStep<string, string>
    {
        public string Process(string input)
        {
            return input.Replace(" ", "");
        }
    }
    //Шаг 2: Преобразование в верхний регистр
    public class ToUpperStep : IPipelineStep<string, string>
    {
        public string Process(string input)
        {
            return input.ToUpper();
        }
    }

    //Шаг 3: Добавление префикса
    public class AddPrefixStep : IPipelineStep<string, string>
    {
        public string Process(string input)
        {
            return $"Processed: {input}";
        }
    }

    //3. Класс конвейера
    public class Pipeline<TInput, TOutput>
    {
        private readonly List<object> _steps = new List<object>();

        public Pipeline<TInput, TNewOutput> AddStep<TNewOutput>(IPipelineStep<TOutput, TNewOutput> step)
        {
            var newPipeline = new Pipeline<TInput, TNewOutput>();
            newPipeline._steps.AddRange(_steps);
            newPipeline._steps.Add(step);
            return newPipeline;
        }

        public TOutput Execute(TInput input)
        {
            object currentValue = input;
            foreach (var step in _steps)
            {
                var stepType = step.GetType();
                var method = stepType.GetMethod("Process");
                currentValue = method.Invoke(step, new[] { currentValue });
            }
            return (TOutput)currentValue;
        }
    }

    //4. Использование
    class Program
    {
        static void Main()
        {
            var pipeline = new Pipeline<string, string>()
                .AddStep(new RemoveWhitespaceStep())
                .AddStep(new ToUpperStep())
                .AddStep(new AddPrefixStep());

            string input = "   Hello, Pipeline!   ";
            string result = pipeline.Execute(input);

            Console.WriteLine(result); // Вывод: "Processed: HELLO,PIPELINE!"
        }
    }
}