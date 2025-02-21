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
namespace PatternPipeLine01
{
    //1. Интерфейс шага конвейера
    public interface IPipelineStep<TInput, TOutput>
    {
        TOutput Process(TInput input);
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
}

namespace ExampleForProduction
{
    public class Pipeline<TInput, TOutput>
    {
        // Хранит составную функцию обработки данных
        private readonly Func<TInput, TOutput> _pipelineFunc;

        // Приватный конструктор для создания нового конвейера с переданной функцией
        private Pipeline(Func<TInput, TOutput> pipelineFunc)
        {
            _pipelineFunc = pipelineFunc;
        }
        #region Пояснение конструкции CreateInitialPipeline<T>()
        /*
         Конструкция CreateInitialPipeline<T>() — это статический фабричный метод, который создает начальный (пустой) конвейер
           с одинаковыми типами входных и выходных данных (TInput = TOutput = T).
            Метод CreateInitialPipeline — это отправная точка для построения конвейера.
               После него можно добавлять шаги, которые могут менять тип данных.

        1. Назначение метода
        Этот метод решает две ключевые проблемы:
        Безопасная инициализация конвейера:
        - Гарантирует, что начальный конвейер имеет одинаковые типы TInput и TOutput.
        - Избегает небезопасного приведения типов, которое было в конструкторе по умолчанию.

        Удобство создания конвейера:
        - Позволяет явно указать, что конвейер начинается с типа T и пока не имеет шагов.

         2. Преимущества подхода

        Типобезопасность: Нет риска runtime-ошибок из-за несовместимости типов на старте.
        Ясность:  Код явно указывает, что конвейер начинается с "пустого" преобразования.
        Гибкость: Можно начать с любого типа T и постепенно менять его через шаги.


        Лямбда input => input:
          - Это "пустое" преобразование: принимает значение типа T и возвращает его без изменений. Фактически,
              это единичная функция (identity function), которая ничего не делает, но сохраняет тип данных.
         */
        #endregion
        public static Pipeline<T, T> CreateInitialPipeline<T>()
        {
            return new Pipeline<T, T>(input => input);
        }



        public Pipeline<TInput, TNewOutput>
            AddStep<TNewOutput>(Func<TOutput, TNewOutput> step)
        {
            return new Pipeline<TInput, TNewOutput>(
                input => step(_pipelineFunc(input))
            );
        }

        public TOutput Execute(TInput input)
        {
            var result = _pipelineFunc(input);
            if (result is not TOutput)
            {
                throw new InvalidOperationException("Тип результата не соответствует TOutput.");
            }
            else
            {
                return result;
            }

        }
    }
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
            var pipeline = Pipeline<string, string>
                .CreateInitialPipeline<string>()
                .AddStep(input => input.ToUpper())
                .AddStep(input => input.Replace(" ", ""))
                .AddStep(input => $"[Processed] {input}");


            string s = "   Hello, Pipeline!   ";
            Console.WriteLine(s);
            string result = pipeline.Execute(s);

            Console.WriteLine(result); 
        }
    }
}