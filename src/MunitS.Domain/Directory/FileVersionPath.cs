namespace MunitS.Domain.Directory;

public class ObjectVersionPath(ObjectVersionDirectory objectVersionDirectory, string extension) : DirectoryBase
{
    private const string ObjectVersionFileName = "data";
    public override string Value { get; } = Path.Combine(objectVersionDirectory.Value, $"{ObjectVersionFileName}.{extension}");
}
