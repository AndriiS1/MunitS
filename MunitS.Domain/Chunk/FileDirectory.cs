namespace MunitS.Domain.Chunk;

public class FileDirectory
{
    public string Value { get; }

    public FileDirectory(string rootPath, string fileKey)
    {
        var fileDirectory = fileKey.Split(".").SkipLast(1);
        
        Value = $"{rootPath}/{fileDirectory}";
    }
}
