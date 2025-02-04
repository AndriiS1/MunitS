namespace MunitS.Domain.Chunk;

public class ObjectPath(VersionedObjectDirectory versionedObjectDirectory)
{
    private const string DataDirectory = "Data";
    public string Value { get; } = $"{versionedObjectDirectory.Value}/{DataDirectory}";
}
