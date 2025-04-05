namespace MunitS.Domain.Directory;

public class ObjectVersionDirectory(ObjectDirectory objectDirectory, Guid versionId) : DirectoryBase
{
    public override string Value { get; } = Path.Combine(objectDirectory.Value, versionId.ToString());
}
