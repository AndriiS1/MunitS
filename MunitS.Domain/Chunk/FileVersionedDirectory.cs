namespace MunitS.Domain.Chunk;

public class FileVersionedDirectory
{
    public string Value { get; }
    
    public Guid VersionId { get; }

    public FileVersionedDirectory(FileDirectory fileDirectory)
    {
       VersionId = Guid.NewGuid();
       Value = $"{fileDirectory}/{VersionId}";
    }
}