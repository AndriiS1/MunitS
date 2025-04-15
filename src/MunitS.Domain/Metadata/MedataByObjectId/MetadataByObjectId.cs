namespace MunitS.Domain.Metadata.MedataByObjectId;

public class MetadataByObjectId
{
    public required Guid BucketId { get; init; }
    public required Guid UploadId { get; init; }
    public required Guid ObjectId { get; init; }
    public required string MimeType { get; init; }
    public required long SizeInBytes { get; init; }
    public required Dictionary<string, string> CustomMetadata { get; init; }
    public required Dictionary<string, string> Tags { get; init; }

    public static MetadataByObjectId Create(Guid bucketId, Guid uploadId, Guid objectId, string mimeType, long sizeInBytes)
    {
        return new MetadataByObjectId
        {
            BucketId = bucketId,
            UploadId = uploadId,
            ObjectId = objectId,
            MimeType = mimeType,
            SizeInBytes = sizeInBytes,
            CustomMetadata = new Dictionary<string, string>(),
            Tags = new Dictionary<string, string>()
        };
    }
}
