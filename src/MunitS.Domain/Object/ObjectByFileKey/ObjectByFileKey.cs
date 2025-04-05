using MunitS.Domain.Directory;
namespace MunitS.Domain.Object.ObjectByFileKey;

public class ObjectByFileKey
{
    private const string TempDirectory = "temp";
    private const string CompressionExtension = "gzip";
    public required Guid Id { get; init; }
    public required Guid BucketId { get; init; }
    public required Guid VersionId { get; init; }
    public required Guid UploadId { get; init; }
    public required string FileKey { get; init; }
    public required string FileName { get; init; }
    public DateTimeOffset? UploadedAt { get; init; }
    public required DateTimeOffset InitiatedAt { get; init; }
    public required string UploadStatus { get; init; }
    public required string Path { get; init; }
    public required string Extension { get; init; }

    public static ObjectByFileKey Create(Guid bucketId, string fileKey, string fileName,
        DateTimeOffset initiatedAt, DivisionDirectory divisionDirectory, string extension)
    {
        var id = Guid.NewGuid();
        var versionId = Guid.NewGuid();
        return new ObjectByFileKey
        {
            Id = id,
            BucketId = bucketId,
            FileKey = fileKey,
            FileName = fileName,
            InitiatedAt = initiatedAt,
            UploadId = Guid.NewGuid(),
            VersionId = versionId,
            Path = new ObjectDirectory(divisionDirectory, id).ToString(),
            UploadStatus = Object.ObjectByFileKey.UploadStatus.Instantiated.ToString(),
            Extension = extension
        };
    }

    public string GetObjectPath()
    {
        return $"{Path}/{VersionId}";
    }

    public string GetObjectTempPath()
    {
        return $"{Path}/{VersionId}/{TempDirectory}";
    }
}
