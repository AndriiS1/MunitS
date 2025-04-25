using MunitS.Domain.Division.DivisionByBucketId;
namespace MunitS.Domain.Object.ObjectByUploadId;

public class ObjectByUploadId
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

    public static ObjectByUploadId Create(Guid bucketId, Guid divisionId, Guid objectId, string fileKey, string fileName,
        DateTimeOffset initiatedAt, DivisionType.SizeType divisionSizeType, string extension, string mimeType, long sizeInBytes)
    {
        return new ObjectByUploadId
        {
            Id = objectId,
            BucketId = bucketId,
            FileKey = fileKey,
            FileName = fileName,
            DivisionId = divisionId,
            InitiatedAt = initiatedAt,
            UploadId = Guid.NewGuid(),
            DivisionSizeType = divisionSizeType.ToString(),
            UploadStatus = Object.ObjectByUploadId.UploadStatus.Instantiated.ToString(),
            Extension = extension,
            MimeType = mimeType,
            SizeInBytes = sizeInBytes,
            CustomMetadata = new Dictionary<string, string>(),
            Tags = new Dictionary<string, string>()
        };
    }
}
