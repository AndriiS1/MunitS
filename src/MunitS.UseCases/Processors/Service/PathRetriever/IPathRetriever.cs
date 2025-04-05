using MunitS.Domain.Bucket.BucketById;
using MunitS.Domain.Directory;
namespace MunitS.UseCases.Processors.Service.PathRetriever;

public interface IPathRetriever
{
    BucketDirectory GetAbsoluteBucketDirectory(BucketById bucketByIdName);
}
