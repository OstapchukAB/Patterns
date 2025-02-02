namespace MyClassLib.FactoryMethod
{
    //фабричный метод FileReaderFactory
    public static class FileReaderFactory
    {
        public static IFileReader CreateReader(string filePath)
        {
            string extension = Path.GetExtension(filePath).ToLower();

            return extension switch
            {
                ".txt" => new TxtReader(),
                ".json" => new JsonReader(),
                ".csv" => new CsvReader(),
                _ => throw new NotSupportedException($"Формат '{extension}' не поддерживается.")
            };
        }
    }
}
