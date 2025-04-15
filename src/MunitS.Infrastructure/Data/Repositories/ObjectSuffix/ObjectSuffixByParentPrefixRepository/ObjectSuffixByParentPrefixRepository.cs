using Cassandra.Data.Linq;
using MunitS.Domain.ObjectSuffix.ObjectSuffixByParentPrefix;
using MunitS.Infrastructure.Data.Repositories.ObjectSuffix.ObjectSuffixByParentPrefixRepository.Dtos;
namespace MunitS.Infrastructure.Data.Repositories.ObjectSuffix.ObjectSuffixByParentPrefixRepository;

public class ObjectSuffixByParentPrefixRepository(CassandraConnector connector) : IObjectSuffixByParentPrefixRepository
{
    private readonly Table<ObjectSuffixByParentPrefix> _buckets = new(connector.GetSession());
    
    public async Task<ObjectSuffixesPage> GetPage(Guid bucketId, string parentPrefix, int pageSize, 
        ObjectSuffixesPage.ObjectSuffixesPageCursor? cursor = null)
    {
        var query = _buckets
            .Where(b => b.BucketId == bucketId && b.ParentPrefix == parentPrefix);

        if (cursor is not null)
        {
            query = query
                .Where(b => b.Type.CompareTo(cursor.Type) > 0 ||
                            (b.Type == cursor.Type.ToString() && b.Suffix.CompareTo(cursor.Suffix) > 0));
        }

        var results = (await query
                .Take(pageSize + 1)
                .ExecuteAsync())
            .ToList();

        var hasMore = results.Count > pageSize;
        
        if (hasMore)
        {
            results = results.Take(pageSize).ToList();
        }

        var lastItem = results.LastOrDefault();

        return new ObjectSuffixesPage
        {
            Data = results,
            HasNext = hasMore,
            NextCursor = hasMore && lastItem != null
                ? new ObjectSuffixesPage.ObjectSuffixesPageCursor(Enum.Parse<PrefixType>(lastItem.Type), lastItem.Suffix)
                : null
        };
    }
    
    public async Task Delete(Guid bucketId, string parentPrefix, string suffix)
    {
        await _buckets.Where(o => o.ParentPrefix == parentPrefix && o.BucketId == bucketId && o.Suffix == suffix).Delete().ExecuteAsync();
    }
    
    public  async Task Delete(Guid bucketId, string parentPrefix)
    {
        await _buckets.Where(o => o.ParentPrefix == parentPrefix && o.BucketId == bucketId).Delete().ExecuteAsync();
    }

    public async Task Create(ObjectSuffixByParentPrefix objectSuffixByParentPrefix)
    {
        await _buckets.Insert(objectSuffixByParentPrefix).ExecuteAsync();
    }
    
    public async Task<ObjectSuffixByParentPrefix?> Any(Guid bucketId, string parentPrefix)
    {
       return await _buckets.FirstOrDefault(o => o.ParentPrefix == parentPrefix && o.BucketId == bucketId).ExecuteAsync();
    }
}
