using MunitS.Domain.Chunk;
namespace MunitS.Domain.Metadata;

public class MetadataPath(VersionedObjectDirectory versionedObjectDirectory)
{
    private const string MetadataFileName = "metadata.json";
    public string Value { get;} = $"{versionedObjectDirectory.Value}/{MetadataFileName}";
}
