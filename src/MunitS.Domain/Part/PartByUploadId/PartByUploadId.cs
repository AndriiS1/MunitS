namespace MunitS.Domain.Part.PartByUploadId;

public class PartByUploadId
{
    public required Guid Id { get; init; }
    public required Guid BucketId { get; init; }
    public required Guid UploadId { get; init; }
    public required string ETag { get; init; }
    public required int Number { get; init; }

    public static PartByUploadId Create(Guid bucketId, Guid uploadId, string eTag, int number)
    {
        return new PartByUploadId
        {
            Id = Guid.NewGuid(),
            BucketId = bucketId,
            UploadId = uploadId,
            ETag = eTag,
            Number = number
        };
    }
}
