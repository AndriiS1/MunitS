using MunitS.Domain.Chunk;
namespace MunitS.Domain.Metadata;

public class MetadataPath(FileVersionedDirectory fileVersionedDirectory)
{
    private const string MetadataFileName = "metadata.json";
    public string Value { get;} = $"{fileVersionedDirectory.Value}/{MetadataFileName}";
}
