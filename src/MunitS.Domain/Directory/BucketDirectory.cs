using MunitS.Domain.Bucket.BucketById;
namespace MunitS.Domain.Directory;

public class BucketDirectory(string rootPath, BucketById bucketById) : DirectoryBase
{
    public override string Value { get; } = Path.Combine(rootPath, bucketById.Name);
}
