using Cassandra.Data.Linq;
using MunitS.Domain.Versioning;
namespace MunitS.Infrastructure.Data.Repositories.ObjectVersioning;

public class ObjectVersionRepository(CassandraConnector connector): IObjectVersionRepository
{
    private readonly Table<ObjectVersion> _objectVersions = new (connector.GetSession());
    public async Task Create(ObjectVersion version)
    {
        await _objectVersions.Insert(version).ExecuteAsync();
    }
    
    public async Task<List<ObjectVersion>> GetAll(Guid objectId)
    {
        return (await _objectVersions.Where(x => x.ObjectId == objectId).ExecuteAsync()).ToList();
    }
    
    private async Task<ObjectVersion?> GetOldestVersionAsync(Guid objectId)
    {
        return await _objectVersions.OrderByDescending(v => v.UploadedAt).FirstOrDefault().ExecuteAsync();

    }
    
    public async Task DeleteOldest(Guid objectId)
    {
        var oldestVersion = await GetOldestVersionAsync(objectId);
        
        if (oldestVersion == null) return;

        _objectVersions.Where(v => v.Id == oldestVersion.Id).Delete();
    }
}
