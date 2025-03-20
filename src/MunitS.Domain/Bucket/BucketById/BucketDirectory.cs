namespace MunitS.Domain.Bucket.BucketById;

public class BucketDirectory(string rootPath, Bucket.BucketById.BucketById bucketById)
{
    public string Value { get; } = $"{rootPath}/{bucketById.Name}";
}
