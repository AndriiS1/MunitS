namespace MunitS.Domain.Chunk;

public class VersionedObjectDirectory(ObjectDirectory objectDirectory, Guid versionId)
{
    public string Value { get; } = $"{objectDirectory.Value}/{versionId}";

    public Guid VersionId { get; } = versionId;
}