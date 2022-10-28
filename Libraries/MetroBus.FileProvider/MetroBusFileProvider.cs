namespace MetroBus.FileProvider;

public class MetroBusFileProvider : IMetroBusFileProvider 
{
    public void SaveFile(byte[] bytes, string filePath)
    {
        using var stream = File.Create(filePath);
        stream.Write(bytes, 0, bytes.Length);
    }
    
    
    public void CombineFiles(string inputDirectoryPath,
        string outputFilePath)
    {
        string[] inputFilePaths = Directory.GetFiles(inputDirectoryPath).OrderBy(x=>x).ToArray();
        using (var outputStream = File.Create(outputFilePath))
        {
            foreach (var inputFilePath in inputFilePaths)
            {
                using (var inputStream = File.OpenRead(inputFilePath))
                {
                    // Buffer size can be passed as the second argument.
                    inputStream.CopyTo(outputStream);
                }
                Console.WriteLine("The file {0} has been processed.", inputFilePath);
            }
        }
    }

    public async Task<byte[]> ReadAllBytes(string fileName)
    {
        return await File.ReadAllBytesAsync(fileName);
    }

    public string GetFileName(string path)
    {
        return path.Split("/").LastOrDefault();
    }

    public void DeleteDirectory(string directoryName)
    {
        var files = Directory.GetFiles(directoryName);

        foreach (var file in files)
        {
            File.Delete(file);
        }
        
        Directory.Delete(directoryName);
    }
}