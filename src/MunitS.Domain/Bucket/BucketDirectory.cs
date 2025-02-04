namespace MunitS.Domain.Bucket;

public class BucketDirectory(string name)
{
    public string Value { get; } = $"{name}";
}
