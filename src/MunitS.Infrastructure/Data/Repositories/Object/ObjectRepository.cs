using Cassandra.Data.Linq;
namespace MunitS.Infrastructure.Data.Repositories.Object;

public class ObjectRepository(CassandraConnector connector): IObjectRepository
{
    private readonly Table<Domain.Object.Object> _objects = new (connector.GetSession());
    
    public async Task<Domain.Object.Object?> Get(string fileKey, Guid bucketId)
    {
        return await _objects.FirstOrDefault(o => o.BucketId == bucketId && o.FileKey == fileKey).ExecuteAsync();
    }
    
    public async Task<List<Domain.Object.Object>> GetAll(string fileKey, Guid bucketId)
    {
        return (await _objects.Where(o => o.BucketId == bucketId && o.FileKey == fileKey).ExecuteAsync()).ToList();
    }
    
    public async Task Delete(string fileKey, Guid bucketId, Guid versionId)
    {
        await _objects.Where(o => o.FileKey == fileKey && o.BucketId == bucketId && o.VersionId == versionId).Delete().ExecuteAsync();
    }
    
    public async Task Create(Domain.Object.Object @object)
    {
        await _objects.Insert(@object).ExecuteAsync(); 
    }
}
