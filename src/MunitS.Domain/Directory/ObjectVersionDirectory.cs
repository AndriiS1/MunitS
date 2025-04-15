namespace MunitS.Domain.Directory;

public class ObjectVersionDirectory(ObjectDirectory objectDirectory, Guid uploadId) : DirectoryBase
{
    public override string Value { get; } = Path.Combine(objectDirectory.Value, uploadId.ToString());
}
