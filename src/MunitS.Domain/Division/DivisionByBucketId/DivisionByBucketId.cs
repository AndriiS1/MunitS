namespace MunitS.Domain.Division.DivisionByBucketId;

public class DivisionByBucketId
{
    public required Guid BucketId { get; init; }
    public required string Type { get; init; }
    public required Guid Id { get; init; }
    public required long ObjectsLimit { get; init; }

    public DivisionType.SizeType GetSizeType()
    {
        return Enum.Parse<DivisionType.SizeType>(Type);
    }

    public static DivisionByBucketId Create(Guid bucketId, string bucketName, DivisionType divisionType)
    {
        var id = Guid.NewGuid();
        return new DivisionByBucketId
        {
            Id = id,
            BucketId = bucketId,
            ObjectsLimit = divisionType.ObjectsCountLimit,
            Type = divisionType.Type.ToString()
        };
    }
}
