using MunitS.Domain.Bucket.BucketById;
namespace MunitS.Infrastructure.Data.Repositories.Bucket.BucketByIdRepository;

public interface IBucketByIdRepository
{
    Task<BucketById?> Get(Guid bucketId);
    Task<List<BucketById>> GetAll(Guid[] bucketIds);
    Task Create(BucketById bucketById);
    Task Delete(Guid bucketId);
}
