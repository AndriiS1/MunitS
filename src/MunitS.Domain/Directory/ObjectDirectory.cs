namespace MunitS.Domain.Directory;

public class ObjectDirectory(DivisionDirectory divisionDirectory, Guid objectId) : DirectoryBase
{
    public override string Value { get; } = Path.Combine(divisionDirectory.Value, objectId.ToString());
}
