namespace MunitS.Infrastructure.Data.Repositories.Bucket;

public interface IBucketRepository
{ 
    Task<Domain.Bucket.Bucket?> Get(string name);
}
