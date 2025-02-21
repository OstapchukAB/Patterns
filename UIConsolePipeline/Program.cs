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

#region Пример применения Паттерна -конвейер для обработки текста
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
     // TInput —  тип входных данных конвейера.
     // TOutput — тип выходных данных после выполнения всех шагов.
     // _steps —  список объектов, представляющих шаги конвейера.
    public class Pipeline<TInput, TOutput>
    {
        private readonly List<object> _steps = new List<object>();

        #region Описание работы метода AddStep
        /*
        Назначение: 
            Добавляет новый шаг в конвейер и возвращает новый экземпляр конвейера с обновленным типом выходных данных (TNewOutput).

        Как работает:
            Создает новый конвейер (newPipeline) с типом вывода TNewOutput.
            Копирует все текущие шаги (_steps) из исходного конвейера в новый.
            Добавляет переданный шаг (step) в новый конвейер.
            Возвращает новый конвейер.

        Особенности:
            Метод не изменяет текущий конвейер, а создает новый. Это позволяет строить цепочки шагов через fluent-интерфейс (например, .AddStep(...).AddStep(...)).
            Каждый новый шаг "знает", что его входной тип (TOutput) совпадает с выходным типом предыдущего шага.
         */
        #endregion
        public Pipeline<TInput, TNewOutput> AddStep<TNewOutput>(IPipelineStep<TOutput, TNewOutput> step)
        {
            var newPipeline = new Pipeline<TInput, TNewOutput>();
            newPipeline._steps.AddRange(_steps);
            newPipeline._steps.Add(step);
            return newPipeline;
        }
        #region Описание работы метода Execute
        /*
        Назначение: 
            Выполняет все шаги конвейера последовательно, передавая результат каждого шага следующему.

        Как работает:
            Инициализирует currentValue входными данными (input).

        Для каждого шага в _steps:
            Получает тип шага (stepType).
            Находит метод Process через рефлексию.
            Вызывает метод Process, передавая currentValue.
            Обновляет currentValue результатом вызова.

        Возвращает итоговое значение, приведенное к TOutput.

        Особенности:
            Использует рефлексию (GetMethod, Invoke), что может снижать производительность. В реальных проектах это можно заменить на компиляцию выражений или делегаты.
            Не проверяет типы данных на этапе компиляции. Если типы шагов несовместимы, возникнет ошибка во время выполнения.
         */
        #endregion
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

        #region 5. Проблемы и ограничения данной реализации
        /*
        Рефлексия:
           Метод Execute использует Invoke, что может быть медленно.
           Решение: Заменить рефлексию на компиляцию делегатов через Expression или использовать dynamic.

        Безопасность типов:
           Нет проверки на этапе компиляции, что типы шагов совместимы.
           Ошибки (например, несовпадение TInput шага с TOutput предыдущего) проявятся только во время выполнения.

        Иммутабельность:
           Метод AddStep создает новый конвейер, что может быть неэффективно при частых изменениях.
         */
        #endregion
    }

    //6. Альтернативная реализация (без рефлексии)
    //Чтобы избежать рефлексии, можно использовать делегаты:
    //Шаги добавляются как делегаты Func<TOutput, TNewOutput>, что обеспечивает безопасность типов и повышает производительность.
    //public class PipelineWithDelegate<TInput, TOutput>
    //{
    //    private readonly List<Func<object, object>> _steps = new List<Func<object, object>>();

    //    public PipelineWithDelegate<TInput, TNewOutput> AddStep<TNewOutput>(Func<TOutput, TNewOutput> step)
    //    {
    //        var newPipeline = new PipelineWithDelegate<TInput, TNewOutput>();
    //        newPipeline._steps.AddRange(_steps);
    //        newPipeline._steps.Add(o => step((TOutput)o));
    //        return newPipeline;
    //    }

    //    public TOutput Execute(TInput input)
    //    {
    //        object current = input;
    //        foreach (var step in _steps)
    //        {
    //            current = step(current);
    //        }
    //        return (TOutput)current;
    //    }
    //}

    //4. Использование
    class Program
    {
        static void Main()
        {

            #region Описание
            /*
           Каждый вызов AddStep создает новый конвейер, "наследующий" предыдущие шаги.
           Тип TNewOutput автоматически выводится на основе шага. Например, если шаг возвращает int, то новый конвейер будет Pipeline<string, int>.
             */
            #endregion
            var pipeline = new Pipeline<string, string>()
                .AddStep(new RemoveWhitespaceStep())
                .AddStep(new ToUpperStep())
                .AddStep(new AddPrefixStep());

            //Вариант с делегатами не работает
            //var pipeline= new PipelineWithDelegate<string,string>();
            //pipeline.AddStep(new RemoveWhitespaceStep().Process);
            //pipeline.AddStep(new ToUpperStep().Process);
            //pipeline.AddStep(new AddPrefixStep().Process);

            string input = "   Hello, Pipeline!   ";
            Console.WriteLine(input);
            string result = pipeline.Execute(input);

            Console.WriteLine(result); // Вывод: "Processed: HELLO,PIPELINE!"
        }
    }
}