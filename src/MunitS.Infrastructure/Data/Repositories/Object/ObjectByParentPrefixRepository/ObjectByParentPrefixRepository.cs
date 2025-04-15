using Cassandra.Data.Linq;
using MunitS.Domain.Object.ObjectByParentPrefix;
namespace MunitS.Infrastructure.Data.Repositories.Object.ObjectByParentPrefixRepository;

public class ObjectByParentPrefixRepository(CassandraConnector connector) : IObjectByParentPrefixRepository
{
    private readonly Table<ObjectByParentPrefix> _objects = new(connector.GetSession());

    public async Task<List<ObjectByParentPrefix>> GetAll(Guid bucketId, string parentPrefix)
    {
        return (await _objects.Where(o => o.BucketId == bucketId && o.ParentPrefix == parentPrefix).ExecuteAsync()).ToList();
    }

    public async Task Delete(Guid bucketId,  string fileName, string parentPrefix)
    {
        await _objects.Where(o => o.ParentPrefix == parentPrefix && o.BucketId == bucketId && o.FileName == fileName).Delete().ExecuteAsync();
    }

    public async Task Create(ObjectByParentPrefix objectByParentPrefix)
    {
        await _objects.Insert(objectByParentPrefix).ExecuteAsync();
    }
}
