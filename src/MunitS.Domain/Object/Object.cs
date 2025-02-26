namespace MunitS.Domain.Object;

public class Object
{
    public const string TableName = "objects";
    public required Guid Id { get; init; }
    public required Guid BucketId { get; init; } 
    public required Guid VersionId { get; init; } 
    public required string FileKey { get; init; }
    public required string FileName { get; init; }
    public required DateTimeOffset UploadedAt { get; init; }
    public static Object Create(Guid bucketId, string fileKey, string fileName, DateTimeOffset uploadedAt)
    {
        return new Object
        {
            Id = Guid.NewGuid(),
            BucketId = bucketId,
            FileKey = fileKey,
            FileName = fileName,
            UploadedAt = uploadedAt,
            VersionId = Guid.NewGuid(),
        };
    }
}
