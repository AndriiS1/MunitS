namespace MunitS.Domain.Object.ObjectByFileKey;

public class ObjectByFileKey
{
    public required Guid Id { get; init; }
    public required Guid BucketId { get; init; }
    public required string FileKey { get; init; }
    public DateTimeOffset CreatedAt { get; private init; }

    public static ObjectByFileKey Create(Guid bucketId, Guid objectId, string fileKey)
    {
        return new ObjectByFileKey
        {
            Id = objectId,
            BucketId = bucketId,
            FileKey = fileKey,
            CreatedAt = DateTimeOffset.UtcNow
        };
    }
}
