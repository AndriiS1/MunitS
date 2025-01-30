namespace MunitS.Domain.Chunk;

public class FilePath
{
    public string Value { get; }
    public Guid VersionId { get; }
    
    public FilePath(string rootPath, string fileKey)
    {
        VersionId = Guid.NewGuid();
        Value = $"{new FileDirectory(rootPath, fileKey).Value}/{VersionId}/{GetFileName(fileKey)}";
    }
    
    private static string GetFileName(string fileKey)
    {
        return fileKey.Split('/').Last();
    }
}
