namespace MunitS.Infrastructure.Data.Repositories.Bucket.BucketCounter;

public interface IBucketCounterRepository
{
    Task<Domain.Bucket.BucketCounter.BucketCounter?> Get(Guid bucketId);
    Task IncrementObjectsCount(Guid bucketId, long increment = 1);
    Task IncrementSizeInBytesCount(Guid bucketId, long increment);
    Task<List<Domain.Bucket.BucketCounter.BucketCounter>> GetAll(IEnumerable<Guid> bucketIds);
}
