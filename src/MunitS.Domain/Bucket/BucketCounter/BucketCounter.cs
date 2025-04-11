namespace MunitS.Domain.Bucket.BucketCounter;

public class BucketCounter
{
    public required Guid Id { get; init; }
    public required long ObjectsCount { get; init; }
    public required long SizeInBytes { get; init; }

    public static BucketCounter Empty(Guid bucketId)
    {
        return new BucketCounter
        {
            Id = bucketId,
            ObjectsCount = 0,
            SizeInBytes = 0
        };
    }
}
