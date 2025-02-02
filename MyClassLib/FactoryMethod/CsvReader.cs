namespace MyClassLib.FactoryMethod
{
    public class CsvReader : IFileReader
    {
        public string Read(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"Файл '{filePath}' не найден.");

            var lines = File.ReadAllLines(filePath);
            return string.Join(Environment.NewLine, lines.Select(line => string.Join("\t", line.Split(','))));
        }
    }
}
