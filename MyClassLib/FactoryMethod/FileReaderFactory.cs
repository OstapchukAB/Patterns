namespace MyClassLib.FactoryMethod
{
    //Фабричный метод: Класс FileReaderFactory использует фабричный метод для создания экземпляров IFileReader на основе расширения файла.
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
