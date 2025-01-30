using MunitS.Domain.Chunk;
namespace MunitS.Domain.Metadata;

public class Metadata(FileVersionedDirectory fileVersionedDirectory,
    ulong fileSize, uint version, string fileName, DateTime lastModified, DateTime uploaded)
{
    private MetadataPath Path { get; } = new(fileVersionedDirectory);
    public FileVersionedDirectory FileVersionedDirectory { get; } = fileVersionedDirectory;
    public required ulong FileSize = fileSize;
    public required uint Version = version;
    public required string FileName = fileName;
    public required DateTime LastModified = lastModified;
    public required DateTime Uploaded = uploaded;

    public string GetPath()
    {
        return Path.Value;
    }
}
