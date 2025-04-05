using MunitS.Domain.Object.ObjectByFileKey;
namespace MunitS.Domain.Directory;

public class TempObjectVersionDirectory(ObjectVersionDirectory objectVersionDirectory) : DirectoryBase
{
    public override string Value { get; } = Path.Combine(objectVersionDirectory.Value, ObjectByFileKey.TempDirectory);
}
