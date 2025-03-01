using MunitS.Domain.Division;
namespace MunitS.Domain.Object;

public class ObjectDirectory(DivisionDirectory divisionDirectory, Guid objectId, Guid versionId)
{
    public string Value { get; } = $"{divisionDirectory.Value}/{objectId}.{versionId}";
}
