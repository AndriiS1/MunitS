using Cassandra.Data.Linq;
namespace MunitS.Infrastructure.Data.Repositories.Object;

public class ObjectRepository(CassandraConnector connector): IObjectRepository
{
    private readonly Table<Domain.Object.ObjectByFileKey.ObjectByFileKey> _objects = new (connector.GetSession());
    
    public async Task<Domain.Object.ObjectByFileKey.ObjectByFileKey?> Get(string fileKey, Guid bucketId)
    {
        return await _objects.FirstOrDefault(o => o.BucketId == bucketId && o.FileKey == fileKey).ExecuteAsync();
    }
    
    public async Task<List<Domain.Object.ObjectByFileKey.ObjectByFileKey>> GetAll(string fileKey, Guid bucketId)
    {
        return (await _objects.Where(o => o.BucketId == bucketId && o.FileKey == fileKey).ExecuteAsync()).ToList();
    }
    
    public async Task Delete(string fileKey, Guid bucketId, Guid versionId)
    {
        await _objects.Where(o => o.FileKey == fileKey && o.BucketId == bucketId && o.VersionId == versionId).Delete().ExecuteAsync();
    }
    
    public async Task Create(Domain.Object.ObjectByFileKey.ObjectByFileKey objectByFileKey)
    {
        await _objects.Insert(objectByFileKey).ExecuteAsync(); 
    }
}
