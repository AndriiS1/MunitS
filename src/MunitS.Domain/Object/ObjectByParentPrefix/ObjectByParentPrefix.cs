namespace MunitS.Domain.Object.ObjectByParentPrefix;

public class ObjectByParentPrefix
{
    public required Guid Id { get; init; }
    public required Guid BucketId { get; init; }
    public required string FileName { get; init; }
    public required string ParentPrefix { get; init; }
    public DateTimeOffset? UploadedAt { get; init; }
    public required DateTimeOffset InitiatedAt { get; init; }

    public static ObjectByParentPrefix Create(Guid id, Guid bucketId, string fileName, string parentPrefix, DateTimeOffset initiatedAt)
    {
        return new ObjectByParentPrefix
        {
            Id = id,
            BucketId = bucketId,
            ParentPrefix = parentPrefix,
            InitiatedAt = initiatedAt,
            FileName = fileName
        };
    }
}
