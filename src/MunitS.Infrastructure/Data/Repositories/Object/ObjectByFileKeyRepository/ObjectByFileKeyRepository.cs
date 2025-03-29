using Cassandra.Data.Linq;
using MunitS.Domain.Object.ObjectByFileKey;
namespace MunitS.Infrastructure.Data.Repositories.Object.ObjectByFileKeyRepository;

public class ObjectByFileKeyRepository(CassandraConnector connector) : IObjectByFileKeyRepository
{
    private readonly Table<ObjectByFileKey> _objects = new(connector.GetSession());

    public async Task<ObjectByFileKey?> Get(string fileKey, Guid bucketId)
    {
        return await _objects.FirstOrDefault(o => o.BucketId == bucketId && o.FileKey == fileKey).ExecuteAsync();
    }

    public async Task<List<ObjectByFileKey>> GetAll(string fileKey, Guid bucketId)
    {
        return (await _objects.Where(o => o.BucketId == bucketId && o.FileKey == fileKey).ExecuteAsync()).ToList();
    }

    public async Task Delete(string fileKey, Guid bucketId, Guid versionId)
    {
        await _objects.Where(o => o.FileKey == fileKey && o.BucketId == bucketId && o.VersionId == versionId).Delete().ExecuteAsync();
    }

    public async Task Create(ObjectByFileKey objectByFileKey)
    {
        await _objects.Insert(objectByFileKey).ExecuteAsync();
    }
}
