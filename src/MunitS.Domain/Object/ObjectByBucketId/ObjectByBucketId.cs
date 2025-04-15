using MunitS.Domain.Division.DivisionByBucketId;
namespace MunitS.Domain.Object.ObjectByBucketId;

public class ObjectByBucketId
{
    public required Guid Id { get; init; }
    public required Guid BucketId { get; init; }
    public required Guid UploadId { get; init; }
    public required Guid DivisionId { get; init; }
    public required string FileKey { get; init; }
    public required string FileName { get; init; }
    public required string DivisionSizeType { get; init; }
    public DateTimeOffset? UploadedAt { get; init; }
    public required DateTimeOffset InitiatedAt { get; init; }
    public required string UploadStatus { get; init; }
    public required string Extension { get; init; }
    public required Dictionary<string, string> CustomMetadata { get; init; }
    public required Dictionary<string, string> Tags { get; init; }
    public required long SizeInBytes { get; init; }
    public required string MimeType { get; init; }

    public static ObjectByBucketId Create(Guid bucketId, Guid divisionId, string fileKey, string fileName,
        DateTimeOffset initiatedAt, DivisionType.SizeType divisionSizeType, string extension, string mimeType, long sizeInBytes)
    {
        return new ObjectByBucketId
        {
            Id = Guid.NewGuid(),
            BucketId = bucketId,
            FileKey = fileKey,
            FileName = fileName,
            DivisionId = divisionId,
            InitiatedAt = initiatedAt,
            UploadId = Guid.NewGuid(),
            DivisionSizeType = divisionSizeType.ToString(),
            UploadStatus = Object.ObjectByBucketId.UploadStatus.Instantiated.ToString(),
            Extension = extension,
            MimeType = mimeType,
            SizeInBytes = sizeInBytes,
            CustomMetadata = new Dictionary<string, string>(),
            Tags = new Dictionary<string, string>()
        };
    }
}
