using MunitS.Domain.Division.DivisionByBucketId;
namespace MunitS.Domain.Object.ObjectByFileKey;

public class ObjectDirectory(DivisionDirectory divisionDirectory, Guid objectId, Guid versionId)
{
    public string Value { get; } = $"{divisionDirectory.Value}/{objectId}.{versionId}";
}
