using MyClassLib.FactoryMethod;

namespace MyClassLib
{
    public class FileService:IFileService
    {
        //Внедряем логирование в FileService
        private readonly FileLogger _logger = FileLogger.Instance;

        // Получить список файлов в папке
        public string[] GetFiles(string directoryPath)
        {
            try
            {
                if (!Directory.Exists(directoryPath))
                    throw new DirectoryNotFoundException($"Папка '{directoryPath}' не найдена.");

                var files = Directory.GetFiles(directoryPath);
                _logger.LogInfo($"Получен список файлов в папке: {directoryPath}");
                return files;
            }
            catch (Exception ex)
            {

                _logger.LogError($"Ошибка при получении файлов в папке: {directoryPath}", ex);
                throw;
            }
           
        }

        // Прочитать содержимое файла
        public string ReadFile(string filePath)
        {
            try
            {
                IFileReader reader = FileReaderFactory.CreateReader(filePath);
                var content = reader.Read(filePath);
                _logger.LogInfo($"Прочитан файл: {filePath}");
                return content;
            }
            catch (Exception ex)
            {

                _logger.LogError($"Ошибка при чтении файла: {filePath}", ex);
                throw;
            }
        }

        // Удалить файл
        public void DeleteFile(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                    throw new FileNotFoundException($"Файл '{filePath}' не найден.");

                File.Delete(filePath);
                _logger.LogInfo($"Файл удалён: {filePath}");
            }
            catch (Exception ex)
            {

                _logger.LogError($"Ошибка при удалении файла: {filePath}", ex);
                throw;
            }
        }
    }
}
