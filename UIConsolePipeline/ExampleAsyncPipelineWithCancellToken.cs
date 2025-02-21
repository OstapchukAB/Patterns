namespace ExampleAsyncPipelineWithCancellToken
{
    // Асинхронный конвейер с поддержкой отмены операций.
    public class AsyncPipeline<TInput, TOutput>
    {
        // Функция, представляющая композицию всех шагов конвейера.
        // Принимает входное значение и токен отмены, возвращает задачу с результатом.
        private readonly Func<TInput, CancellationToken, Task<TOutput>> _pipelineFunc;

        // Приватный конструктор принимает уже составленную асинхронную функцию обработки.
        private AsyncPipeline(Func<TInput, CancellationToken, Task<TOutput>> pipelineFunc)
        {
            _pipelineFunc = pipelineFunc;
        }

        // Фабричный метод для создания начального конвейера.
        // Здесь входной и выходной типы совпадают, и используется identity-функция, обёрнутая в Task.FromResult.
        public static AsyncPipeline<T, T> CreateInitialPipeline<T>()
        {
            return new AsyncPipeline<T, T>((input, cancellationToken) => Task.FromResult(input));
        }

        // Метод AddStep позволяет добавить новый асинхронный шаг в конвейер.
        // Параметр step — функция, принимающая результат предыдущего шага и CancellationToken, возвращает Task с новым результатом.
        public AsyncPipeline<TInput, TNewOutput> AddStep<TNewOutput>(Func<TOutput, CancellationToken, Task<TNewOutput>> step)
        {
            // Композиция функций:
            // 1. Выполняется текущая цепочка (_pipelineFunc) для получения результата типа TOutput.
            // 2. Затем этот результат передается в новую асинхронную функцию step, которая возвращает Task<TNewOutput>.
            return new AsyncPipeline<TInput, TNewOutput>(async (input, cancellationToken) =>
            {
                TOutput currentResult = await _pipelineFunc(input, cancellationToken);
                return await step(currentResult, cancellationToken);
            });
        }

        // Метод Execute запускает конвейер с заданными входными данными и токеном отмены.
        public Task<TOutput> Execute(TInput input, CancellationToken cancellationToken = default)
        {
            return _pipelineFunc(input, cancellationToken);
        }
    }

    class Program
    {
        // Асинхронный Main для использования await и поддержки отмены.
        static async Task Main()
        {
            // Создаем асинхронный конвейер, где вход и выход — string.
            var asyncPipeline = AsyncPipeline<string, string>
                .CreateInitialPipeline<string>()
                // Первый шаг: перевод строки в верхний регистр с имитацией задержки.
                .AddStep(async (input, ct) =>
                {
                    await Task.Delay(100, ct);
                    return input.ToUpper();
                })
                // Второй шаг: удаление пробелов с имитацией задержки.
                .AddStep(async (input, ct) =>
                {
                    await Task.Delay(100, ct);
                    return input.Replace(" ", "");
                })
                // Третий шаг: добавление префикса "[Processed] " с имитацией задержки.
                .AddStep(async (input, ct) =>
                {
                    await Task.Delay(100, ct);
                    return $"[Processed] {input}";
                });

            string s = "   Hello, Async Pipeline!   ";
            Console.WriteLine("Исходная строка: " + s);

            // Создаем CancellationTokenSource для управления отменой.
            using (var cts = new CancellationTokenSource())
            {
                try
                {
                    // Передаем токен отмены при выполнении конвейера.
                    string result = await asyncPipeline.Execute(s, cts.Token);
                    Console.WriteLine("Результат: " + result);
                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine("Операция была отменена.");
                }
            }
        }
    }
}
