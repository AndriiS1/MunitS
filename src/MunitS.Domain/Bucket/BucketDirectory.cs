namespace MunitS.Domain.Bucket;

public class BucketDirectory(string rootPath, string name)
{
    public string Value { get; } = $"{rootPath}/{name}";
}
