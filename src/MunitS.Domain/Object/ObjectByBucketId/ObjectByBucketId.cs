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
    public required DivisionType.SizeType DivisionSizeType { get; init; }
    public DateTimeOffset? UploadedAt { get; init; }
    public required DateTimeOffset InitiatedAt { get; init; }
    public required UploadStatus UploadStatus { get; init; }
    public required string Extension { get; init; }

    public static ObjectByBucketId Create(Guid bucketId, Guid divisionId, string fileKey, string fileName,
        DateTimeOffset initiatedAt, DivisionType.SizeType divisionSizeType, string extension)
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
            DivisionSizeType = divisionSizeType,
            UploadStatus = Object.ObjectByBucketId.UploadStatus.Instantiated,
            Extension = extension
        };
    }
}
