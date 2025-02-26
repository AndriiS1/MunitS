using Cassandra.Data.Linq;
namespace MunitS.Infrastructure.Data.Repositories.Bucket;

public class BucketRepository(CassandraConnector connector): IBucketRepository
{
    private readonly Table<Domain.Bucket.Bucket> _buckets = new (connector.GetSession());
    
    public async Task<Domain.Bucket.Bucket?> Get(string bucketName)
    {
        return await _buckets.FirstOrDefault(b => b.Name == bucketName).ExecuteAsync();
    }
    
    public async Task<List<Domain.Bucket.Bucket>> GetAll(string[] bucketNames)
    {
        return (await _buckets.Where(b => bucketNames.Contains(b.Name)).ExecuteAsync()).ToList();
    }

    public async Task Create(Domain.Bucket.Bucket bucket)
    {
        await _buckets.Insert(bucket).ExecuteAsync();
    }
    
    public async Task Delete(string bucketName)
    {
        await _buckets.Where(b => b.Name == bucketName).Delete().ExecuteAsync();
    }
}
