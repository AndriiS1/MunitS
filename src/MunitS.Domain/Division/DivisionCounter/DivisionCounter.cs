using MunitS.Domain.Division.DivisionByBucketId;
namespace MunitS.Domain.Division.DivisionCounter;

public class DivisionCounter
{
    public required Guid BucketId { get; init; }
    public required string Type { get; init; }
    public required Guid Id { get; init; }
    public required long ObjectsCount { get; init; }

    public static DivisionCounter Create(Guid bucketId, Guid id, DivisionType divisionType)
    {
        return new DivisionCounter
        {
            Id = id,
            BucketId = bucketId,
            Type = divisionType.Type.ToString(),
            ObjectsCount = 0
        };
    }
}
