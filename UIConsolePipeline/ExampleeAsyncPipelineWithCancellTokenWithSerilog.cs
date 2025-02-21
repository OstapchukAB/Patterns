using Serilog; // Подключение библиотеки Serilog для логгирования

namespace ExampleeAsyncPipelineWithCancellTokenWithSerilog
{
    // Асинхронный конвейер с поддержкой отмены и логгирования.
    public class AsyncPipeline<TInput, TOutput>
    {
        // Делегат, представляющий цепочку асинхронных операций.
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
            return new AsyncPipeline<T, T>((input, cancellationToken) =>
            {
                Log.Information("Initial pipeline step: Identity function called with input: {Input}", input);
                return Task.FromResult(input);
            });
        }

        // Метод AddStep позволяет добавить новый асинхронный шаг в конвейер.
        // Параметр step — функция, принимающая результат предыдущего шага и CancellationToken, возвращает Task с новым результатом.
        public AsyncPipeline<TInput, TNewOutput> AddStep<TNewOutput>(Func<TOutput, CancellationToken, Task<TNewOutput>> step)
        {
            // Композиция функций: сначала выполняется текущая цепочка, затем новый шаг.
            return new AsyncPipeline<TInput, TNewOutput>(async (input, cancellationToken) =>
            {
                Log.Information("Executing pipeline step with input: {Input}", input);
                TOutput currentResult = await _pipelineFunc(input, cancellationToken);
                Log.Information("Result after previous steps: {CurrentResult}", currentResult);
                TNewOutput newResult = await step(currentResult, cancellationToken);
                Log.Information("Step completed with output: {NewResult}", newResult);
                return newResult;
            });
        }

        // Метод Execute запускает конвейер с заданными входными данными и токеном отмены.
        public async Task<TOutput> Execute(TInput input, CancellationToken cancellationToken = default)
        {
            Log.Information("Starting pipeline execution with input: {Input}", input);
            TOutput result = await _pipelineFunc(input, cancellationToken);
            Log.Information("Pipeline execution completed with result: {Result}", result);
            return result;
        }
    }

    class Program
    {
        // Асинхронный Main позволяет использовать await.
        static async Task Main()
        {
            // Настройка Serilog: логгирование уровня Debug и вывод в консоль.
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .CreateLogger();

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

            // Завершение работы логгера для корректного сброса всех логов.
            Log.CloseAndFlush();
        }
    }
}
