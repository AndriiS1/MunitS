using MunitS.Domain.Bucket;
namespace MunitS.UseCases.Processors.Service.PathRetriever;

public interface IPathRetriever
{
    BucketDirectory GetBucketDirectory(Bucket bucketName);
}
