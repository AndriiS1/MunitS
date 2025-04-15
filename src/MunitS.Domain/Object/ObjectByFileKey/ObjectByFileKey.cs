namespace MunitS.Domain.Object.ObjectByFileKey;

public class ObjectByFileKey
{
    public required Guid Id { get; init; }
    public required Guid BucketId { get; init; }
    public required Guid UploadId { get; init; }
    public required string FileKey { get; init; }
    public required string UploadStatus { get; init; }
    public DateTimeOffset? UploadedAt { get; init; }

    public static ObjectByFileKey Create(Guid bucketId, Guid objectId, Guid uploadId, string fileKey)
    {
        return new ObjectByFileKey
        {
            Id = objectId,
            BucketId = bucketId,
            FileKey = fileKey,
            UploadId = uploadId,
            UploadStatus = ObjectByBucketId.UploadStatus.Instantiated.ToString()
        };
    }
}
