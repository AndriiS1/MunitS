using Cassandra.Data.Linq;
using MunitS.Domain.Bucket.BucketById;
namespace MunitS.Infrastructure.Data.Repositories.Bucket.BucketByIdRepository;

public class BucketByIdByIdRepository(CassandraConnector connector) : IBucketByIdRepository
{
    private readonly Table<BucketById> _buckets = new(connector.GetSession());

    public async Task<BucketById?> Get(Guid id)
    {
        return await _buckets.FirstOrDefault(b => b.Id == id).ExecuteAsync();
    }

    public async Task<List<BucketById>> GetAll(Guid[] bucketIds)
    {
        return (await _buckets.Where(b => bucketIds.Contains(b.Id)).ExecuteAsync()).ToList();
    }

    public async Task Create(BucketById bucketById)
    {
        await _buckets.Insert(bucketById).ExecuteAsync();
    }

    public async Task Delete(Guid id)
    {
        await _buckets.Where(b => b.Id == id).Delete().ExecuteAsync();
    }
}
