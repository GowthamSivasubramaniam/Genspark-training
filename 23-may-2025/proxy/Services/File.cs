using proxy.Interfaces;
namespace proxy.Services
{

public class File : IFile
{
    private string _filePath;

    public File(string filePath)
    {
        _filePath = filePath;
    }
    
    public string Read()
    {
        return System.IO.File.ReadAllText(_filePath);
    }
    public string ReadMetadata()
    {
        var fileInfo = new System.IO.FileInfo(_filePath);
        return $"File Name: {fileInfo.Name}, Size: {fileInfo.Length} bytes, Created: {fileInfo.CreationTime}";
    }
}
}