using Newtonsoft.Json.Linq;

namespace MyClassLib.FactoryMethod
{
    public class JsonReader : IFileReader
    {
        public string Read(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"Файл '{filePath}' не найден.");

            string json = File.ReadAllText(filePath);
            return JToken.Parse(json).ToString(); // Форматируем JSON для читаемости
        }
    }
}
