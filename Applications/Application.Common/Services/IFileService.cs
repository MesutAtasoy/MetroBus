namespace Application.Common.Services;

public interface IFileService
{
    void SaveFile(byte[] bytes, string filePath);
    void CombineFiles(string[] files, string combinedFile);
    byte[] ReadAllBytes(string fileName);
    string GetFileName(string path);
    void DeleteDirectory(string directoryName);
}