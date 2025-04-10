namespace MunitS.Domain.Directory;

public class PartPath(TempObjectVersionDirectory tempObjectVersionDirectory, int partNumber) : DirectoryBase
{
    private const string PartPrefix = "part";
    public override string Value { get; } = Path.Combine(tempObjectVersionDirectory.Value, $"{PartPrefix}_{partNumber}");
}
