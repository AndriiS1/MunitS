using MunitS.Domain.Chunk;
namespace MunitS.Domain.Metadata;

public class Metadata(FileVersionedDirectory fileVersionedDirectory,
    ulong fileSize, Guid versionId, string fileName, DateTime uploaded)
{
    public MetadataPath Path { get; } = new(fileVersionedDirectory);
    public FileVersionedDirectory FileVersionedDirectory { get; } = fileVersionedDirectory;
    public ulong FileSize = fileSize;
    public string VersionId = versionId.ToString();
    public string FileName = fileName;
    public DateTime Uploaded = uploaded;
}
