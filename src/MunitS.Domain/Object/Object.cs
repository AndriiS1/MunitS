using MunitS.Domain.Chunk;
using MunitS.Domain.Division;
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
    public required string UploadStatus { get; init; }
    public required string Path { get; init; }
    public static Object Create(Guid bucketId, string fileKey, string fileName, DateTimeOffset uploadedAt, DivisionDirectory divisionDirectory)
    {
        var id = Guid.NewGuid();
        var versionId = Guid.NewGuid();
        return new Object
        {
            Id = id,
            BucketId = bucketId,
            FileKey = fileKey,
            FileName = fileName,
            UploadedAt = uploadedAt,
            VersionId = versionId,
            Path = new ObjectDirectory(divisionDirectory, id, versionId).Value,
            UploadStatus = Domain.Object.UploadStatus.Instantiated.ToString(),
        };
    }
}
