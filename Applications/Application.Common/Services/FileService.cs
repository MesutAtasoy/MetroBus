namespace Application.Common.Services;

public class FileService : IFileService 
{
    public void SaveFile(byte[] bytes, string filePath)
    {
        using var stream = File.Create(filePath);
        stream.Write(bytes, 0, bytes.Length);
    }

    public void CombineFiles(string[] files, string combinedFile)
    {
        using BinaryWriter bw = new BinaryWriter(new FileStream(combinedFile, FileMode.Create));
        foreach (var t in files)
            bw.Write(File.ReadAllBytes(t));
    }

    public byte[] ReadAllBytes(string fileName)
    {
        byte[] buffer = null;
        using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
        {
            buffer = new byte[fs.Length];
            fs.Read(buffer, 0, (int)fs.Length);
        }
        return buffer;
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