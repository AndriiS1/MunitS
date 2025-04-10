using Microsoft.Extensions.Options;
using MunitS.Domain.Bucket.BucketById;
using MunitS.Infrastructure.Options.Storage;
namespace MunitS.UseCases.Processors.Service.PathRetriever;

public class PathRetriever(IOptions<StorageOptions> options) : IPathRetriever
{
    public BucketDirectory GetBucketDirectory(BucketById bucketById)
    {
        return new BucketDirectory(options.Value.RootDirectory, bucketById);
    }
}
