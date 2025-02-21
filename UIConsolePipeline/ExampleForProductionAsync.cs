#region  Подробное объяснение ключевых конструкций
/*
   Фабричный метод CreateInitialPipeline
        Создает начальный конвейер, где входной и выходной типы совпадают.
        Использует Task.FromResult(input), чтобы обернуть identity-функцию в задачу, что соответствует асинхронному подходу.

    Метод AddStep
        Принимает функцию типа Func<TOutput, Task<TNewOutput>> — это асинхронный шаг, который преобразует результат предыдущего шага в новый тип.
        Новый шаг добавляется через композицию функций: сначала вызывается текущая функция _pipelineFunc(input), затем результат передается в новую функцию step.
        Благодаря использованию async/await весь процесс становится асинхронным.

    Метод Execute
        Запускает цепочку асинхронных операций, возвращая Task<TOutput>.
        Вызов await asyncPipeline.Execute(s) в Main гарантирует, что выполнение программы дождется завершения всех шагов.

    Эта реализация позволяет легко комбинировать асинхронные операции в конвейере, обеспечивая гибкость, масштабируемость и 
        возможность обрабатывать длительные операции без блокировки потоков.

 */
#endregion
namespace ExampleForProductionAsync
{
    // Класс AsyncPipeline реализует асинхронный паттерн "Конвейер",
    // позволяющий последовательно выполнять асинхронные операции, где каждая операция может менять тип данных.
    public class AsyncPipeline<TInput, TOutput>
    {
        // Делегат, представляющий цепочку асинхронных операций.
        private readonly Func<TInput, Task<TOutput>> _pipelineFunc;

        // Приватный конструктор принимает уже составленную асинхронную функцию обработки.
        private AsyncPipeline(Func<TInput, Task<TOutput>> pipelineFunc)
        {
            _pipelineFunc = pipelineFunc;
        }

        // Фабричный метод для создания начального конвейера.
        // Здесь входной и выходной типы совпадают, и используется identity-функция,
        // обёрнутая в Task.FromResult для соблюдения асинхронного контекста.
        public static AsyncPipeline<T, T> CreateInitialPipeline<T>()
        {
            return new AsyncPipeline<T, T>(input => Task.FromResult(input));
        }

        // Метод AddStep позволяет добавить новый асинхронный шаг в конвейер.
        // Параметр step — функция, которая принимает TOutput и возвращает Task<TNewOutput>.
        public AsyncPipeline<TInput, TNewOutput> AddStep<TNewOutput>(Func<TOutput, Task<TNewOutput>> step)
        {
            // Композиция функций:
            // 1. Выполняется текущая цепочка (_pipelineFunc) для получения результата типа TOutput.
            // 2. Затем этот результат передается в новую асинхронную функцию step,
            //    которая возвращает Task<TNewOutput>.
            return new AsyncPipeline<TInput, TNewOutput>(async input =>
            {
                TOutput currentResult = await _pipelineFunc(input);
                return await step(currentResult);
            });
        }

        // Метод Execute запускает конвейер с заданными входными данными и возвращает результат асинхронно.
        public Task<TOutput> Execute(TInput input)
        {
            return _pipelineFunc(input);
        }
    }

    class Program
    {
        // Асинхронный Main позволяет использовать await.
        async Task Main()
        {
            // Создаем асинхронный конвейер, где вход и выход — string.
            var asyncPipeline = AsyncPipeline<string, string>
                .CreateInitialPipeline<string>()
                // Первый шаг: перевод строки в верхний регистр с имитацией асинхронной задержки.
                .AddStep(async input =>
                {
                    await Task.Delay(100);
                    return input.ToUpper();
                })
                // Второй шаг: удаление пробелов с имитацией задержки.
                .AddStep(async input =>
                {
                    await Task.Delay(100);
                    return input.Replace(" ", "");
                })
                // Третий шаг: добавление префикса "[Processed] " с имитацией задержки.
                .AddStep(async input =>
                {
                    await Task.Delay(100);
                    return $"[Processed] {input}";
                });

            string s = "   Hello, Async Pipeline!   ";
            Console.WriteLine("Исходная строка: " + s);

            // Выполнение конвейера с ожиданием результата.
            string result = await asyncPipeline.Execute(s);
            Console.WriteLine("Результат: " + result);
        }
    }
}
