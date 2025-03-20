using MunitS.Domain.Bucket.BucketByName;
namespace MunitS.Infrastructure.Data.Repositories.Bucket.BucketByNameRepository;

public interface IBucketByNameRepository
{ 
    Task<BucketByName?> Get(string name);
    Task Delete(string name);
    Task Create(BucketByName bucketByName);
}
