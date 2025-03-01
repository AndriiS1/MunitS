namespace MunitS.Domain.Bucket;

public class BucketDirectory(string rootPath, Bucket bucket)
{
    public string Value { get; } = $"{rootPath}/{bucket.Name}";
}
