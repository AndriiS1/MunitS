using Cassandra.Data.Linq;
using MunitS.Domain.FolderPrefixes.FolderPrefixByParentPrefix;
namespace MunitS.Infrastructure.Data.Repositories.FolderPrefix.FolderPrefixByParentPrefixRepository;

public class FolderPrefixByIdByParentPrefixRepository(CassandraConnector connector) : IFolderPrefixByParentPrefixRepository
{
    private readonly Table<FolderPrefixByParentPrefix> _buckets = new(connector.GetSession());

    public async Task<List<FolderPrefixByParentPrefix>> GetAll(Guid bucketId, string parentPrefix)
    {
        return (await _buckets.Where(b => b.ParentPrefix == parentPrefix && b.BucketId == bucketId).ExecuteAsync()).ToList();
    }

    public async Task Delete(Guid bucketId, string parentPrefix, string prefix)
    {
        await _buckets.Where(o => o.ParentPrefix == parentPrefix && o.BucketId == bucketId && o.Prefix == prefix).Delete().ExecuteAsync();
    }

    public async Task Create(FolderPrefixByParentPrefix folderPrefixByParentPrefix)
    {
        await _buckets.Insert(folderPrefixByParentPrefix).ExecuteAsync();
    }
}
