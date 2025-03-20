using MunitS.Domain.Bucket.BucketById;
namespace MunitS.Domain.Division.DivisionByBucketId;

public class DivisionByBucketId
{
    public required Guid BucketId { get; init; }
    public required string Type { get; init; }
    public required string Name { get; init; }
    public required Guid Id { get; init; }
    public required long ObjectsCount { get; init; }
    public required long ObjectsLimit { get; init; }
    public required string Path { get; init; }

    public DivisionType.SizeType GetSizeType()
    {
        return Enum.Parse<DivisionType.SizeType>(Type);
    }
    
    public static DivisionByBucketId Create(Guid bucketId, DivisionType divisionType, BucketDirectory bucketDirectory)
    {
        var id = Guid.NewGuid();
        var name = $"division-{id}";
        return new DivisionByBucketId
        {
            Id = id,
            Name = name,
            BucketId = bucketId,
            ObjectsLimit = divisionType.ObjectsCountLimit,
            Type = divisionType.Type.ToString(),
            ObjectsCount = 0,
            Path = new DivisionDirectory(bucketDirectory, name, divisionType.Type).Value
        };
    }
}
