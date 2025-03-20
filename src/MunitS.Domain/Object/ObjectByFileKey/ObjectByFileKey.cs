using MunitS.Domain.Division.DivisionByBucketId;
namespace MunitS.Domain.Object.ObjectByFileKey;

public class ObjectByFileKey
{
    public const string TableName = "objects";
    private const string CompressionExtension = "gzip";
    public required Guid Id { get; init; }
    public required Guid BucketId { get; init; }
    public required Guid VersionId { get; init; }
    public required string FileKey { get; init; }
    public required string FileName { get; init; }
    public required DateTimeOffset UploadedAt { get; init; }
    public required string UploadStatus { get; init; }
    public required string Path { get; init; }
    public static ObjectByFileKey Create(Guid bucketId, string fileKey, string fileName, DateTimeOffset uploadedAt, DivisionDirectory divisionDirectory)
    {
        var id = Guid.NewGuid();
        var versionId = Guid.NewGuid();
        return new ObjectByFileKey
        {
            Id = id,
            BucketId = bucketId,
            FileKey = fileKey,
            FileName = fileName,
            UploadedAt = uploadedAt,
            VersionId = versionId,
            Path = new ObjectDirectory(divisionDirectory, id, versionId).Value,
            UploadStatus = Object.ObjectByFileKey.UploadStatus.Instantiated.ToString(),
        };
    }

    public string GetObjectDataPath()
    {
        return $"{Path}/{Id}.{VersionId}.{CompressionExtension}";
    }
}
