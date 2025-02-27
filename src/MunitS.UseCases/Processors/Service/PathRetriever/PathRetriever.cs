using Microsoft.Extensions.Options;
using MunitS.Domain.Bucket;
using MunitS.Infrastructure.Options.Storage;
namespace MunitS.UseCases.Processors.Service.PathRetriever;

public class PathRetriever(IOptions<StorageOptions> options) : IPathRetriever
{
    public BucketDirectory GetBucketDirectory(Bucket bucket)
    {
        return new BucketDirectory(options.Value.RootDirectory, bucket);
    }
}
