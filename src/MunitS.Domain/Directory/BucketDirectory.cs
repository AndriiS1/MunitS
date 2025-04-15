namespace MunitS.Domain.Directory;

public class BucketDirectory(string bucketName) : DirectoryBase
{
    public override string Value { get; } = bucketName;
}
