using Cassandra.Data.Linq;
using MunitS.Domain.Object.ObjectByFileKey;
namespace MunitS.Infrastructure.Data.Repositories.Object.ObjectByFileKeyRepository;

public class ObjectByFileKeyRepository(CassandraConnector connector) : IObjectByFileKeyRepository
{
    private readonly Table<ObjectByFileKey> _objects = new(connector.GetSession());

    public async Task<ObjectByFileKey?> Get(Guid bucketId, string fileKey)
    {
        return await _objects.FirstOrDefault(o => o.BucketId == bucketId && o.FileKey == fileKey).ExecuteAsync();
    }

    public async Task Delete(Guid bucketId, string fileKey)
    {
        await _objects.Where(o => o.FileKey == fileKey && o.BucketId == bucketId).Delete().ExecuteAsync();
    }

    public async Task Create(ObjectByFileKey objectByFileKey)
    {
        await _objects.Insert(objectByFileKey).ExecuteAsync();
    }

    public async Task<List<ObjectByFileKey>> GetAll(Guid bucketId, string fileKey)
    {
        return (await _objects.Where(o => o.BucketId == bucketId && o.FileKey == fileKey).ExecuteAsync()).ToList();
    }
    
    public async Task Delete(Guid bucketId)
    {
        await _objects.Where(b => b.BucketId == bucketId).Delete().ExecuteAsync();
    }
}
