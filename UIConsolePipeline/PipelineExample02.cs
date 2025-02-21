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
        static void Main(string[] args)
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
