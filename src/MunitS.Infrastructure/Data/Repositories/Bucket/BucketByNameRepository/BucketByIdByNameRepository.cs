using Cassandra.Data.Linq;
using MunitS.Domain.Bucket.BucketByName;
namespace MunitS.Infrastructure.Data.Repositories.Bucket.BucketByNameRepository;

public class BucketByIdByNameRepository(CassandraConnector connector): IBucketByNameRepository
{
    private readonly Table<BucketByName> _buckets = new (connector.GetSession());
    
    public async Task<BucketByName?> Get(string name)
    {
        return await _buckets.FirstOrDefault(b => b.Name == name).ExecuteAsync();
    }
    
    public async Task Create(BucketByName bucketByName)
    {
        await _buckets.Insert(bucketByName).ExecuteAsync();
    }
    
    public async Task Delete(string name)
    {
        await _buckets.Where(b => b.Name == name).Delete().ExecuteAsync();
    }
}
