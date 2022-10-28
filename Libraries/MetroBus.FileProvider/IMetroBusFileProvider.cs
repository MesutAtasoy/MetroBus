namespace MetroBus.FileProvider;

public interface IMetroBusFileProvider
{
    void SaveFile(byte[] bytes, string filePath);
    void CombineFiles(string inputDirectoryPath, string outputFilePath);
    Task<byte[]> ReadAllBytes(string fileName);
    string GetFileName(string path);
    void DeleteDirectory(string directoryName);
}