using MunitS.Domain.Bucket.BucketById;
namespace MunitS.UseCases.Processors.Service.PathRetriever;

public interface IPathRetriever
{
    BucketDirectory GetBucketDirectory(BucketById bucketByIdName);
}
