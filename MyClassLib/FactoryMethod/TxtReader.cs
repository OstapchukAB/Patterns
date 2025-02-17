namespace FabricMethodClassLib.FactoryMethod
{
    public class TxtReader : IFileReader
    {
        public string Read(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"Файл '{filePath}' не найден.");

            return File.ReadAllText(filePath);
        }
    }
}
