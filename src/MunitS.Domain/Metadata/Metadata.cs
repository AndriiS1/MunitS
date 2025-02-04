using MunitS.Domain.Chunk;
namespace MunitS.Domain.Metadata;

public class Metadata(VersionedObjectDirectory versionedObjectDirectory, string fileKey, 
    ulong fileSize, Guid versionId, string fileName, DateTime uploaded)
{
    public MetadataPath Path { get; } = new(versionedObjectDirectory);
    public VersionedObjectDirectory VersionedObjectDirectory { get; } = versionedObjectDirectory;
    public string FileName { get; } = fileName;
    public string FileKey = fileKey;
    public ulong FileSize = fileSize;
    public string VersionId = versionId.ToString();
    public DateTime Uploaded = uploaded;
    
    // public string ToCreateInstance()
    // {
    //     return $"({Id}, {BucketId}, {FileKey})";
    // }
}
