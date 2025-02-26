namespace MunitS.Infrastructure.Data.Repositories.Bucket;

public interface IBucketRepository
{ 
    Task<Domain.Bucket.Bucket?> Get(string bucketName);
    Task<List<Domain.Bucket.Bucket>> GetAll(string[] bucketNames);
    Task Create(Domain.Bucket.Bucket bucket);
    Task Delete(string bucketName);
}
