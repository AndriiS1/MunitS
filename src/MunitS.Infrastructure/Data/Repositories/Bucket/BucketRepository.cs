using Cassandra;
using Cassandra.Data.Linq;
namespace MunitS.Infrastructure.Data.Repositories.Bucket;

public class BucketRepository(CassandraConnector connector): IBucketRepository
{
    private readonly Table<Domain.Bucket.Bucket> _buckets = new (connector.GetSession());
    
    public async Task<Domain.Bucket.Bucket?> Get(string name)
    {
        return await _buckets.FirstOrDefault(b => b.Name == name).ExecuteAsync();
    }
    
    public async Task Create(Domain.Bucket.Bucket bucket)
    {
        await _buckets.Insert(bucket).ExecuteAsync();
    }
}
