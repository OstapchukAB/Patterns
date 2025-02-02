namespace MyClassLib
{
    public interface IFileService
    {
        string[] GetFiles(string directoryPath);
        string ReadFile(string filePath);
        void DeleteFile(string filePath);
    }
}
