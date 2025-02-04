using MunitS.Domain.Bucket;
namespace MunitS.Domain.Chunk;

public class ObjectDirectory(BucketDirectory bucketDirectory, Guid objectId)
{
    public string Value { get; } = $"{bucketDirectory}/{objectId}";
}
