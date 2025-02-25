using Cassandra.Data.Linq;
namespace MunitS.Infrastructure.Data.Repositories.Object;

public class ObjectRepository(CassandraConnector connector): IObjectRepository
{
    private readonly Table<Domain.Object.Object> _objects = new (connector.GetSession());
    
    public async Task<Domain.Object.Object?> Get(Guid bucketId, string fileKey)
    {
        return await _objects.FirstOrDefault(o => o.BucketId == bucketId && o.FileKey == fileKey).ExecuteAsync();
    }
    
    public async Task Create(Domain.Object.Object @object)
    {
        await _objects.Insert(@object).ExecuteAsync(); 
    }
}
