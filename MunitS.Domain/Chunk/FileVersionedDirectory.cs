namespace MunitS.Domain.Chunk;

public class FileVersionedDirectory
{
    public string Value { get; }
    
    public Guid VersionId { get; }

    public FileVersionedDirectory(ObjectDirectory objectDirectory, Guid versionId)
    {
       VersionId = versionId;
       Value = $"{objectDirectory.Value}/{versionId}";
    }
}