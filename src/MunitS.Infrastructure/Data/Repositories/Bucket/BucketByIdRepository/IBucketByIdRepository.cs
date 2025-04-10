using MunitS.Domain.Bucket.BucketById;
namespace MunitS.Infrastructure.Data.Repositories.Bucket.BucketByIdRepository;

public interface IBucketByIdRepository
{ 
    Task<BucketById?> Get(Guid bucketId);
    Task<List<BucketById>> GetAll(Guid[] bucketIds);
    Task Create(BucketById bucketById);
    Task Delete(Guid bucketId);
    Task IncrementObjectsCount(Guid bucketId, long increment = 1);
    Task IncrementSizeInBytesCount(Guid bucketId, long increment);
}
