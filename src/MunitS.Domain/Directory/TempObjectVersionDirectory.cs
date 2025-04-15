namespace MunitS.Domain.Directory;

public class TempObjectVersionDirectory(ObjectVersionDirectory objectVersionDirectory) : DirectoryBase
{
    private const string TempDirectory = "temp";
    public override string Value { get; } = Path.Combine(objectVersionDirectory.Value, TempDirectory);
}
