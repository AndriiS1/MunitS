using Cassandra.Data.Linq;
using Cassandra.Mapping;
using MunitS.Domain.ObjectSuffix.ObjectSuffixByParentPrefix;
using MunitS.Infrastructure.Data.Repositories.ObjectSuffix.ObjectSuffixByParentPrefixRepository.Dtos;
namespace MunitS.Infrastructure.Data.Repositories.ObjectSuffix.ObjectSuffixByParentPrefixRepository;

public class ObjectSuffixByParentPrefixRepository(CassandraConnector connector) : IObjectSuffixByParentPrefixRepository
{
    private readonly Table<ObjectSuffixByParentPrefix> _objects = new(connector.GetSession());

    public async Task Delete(Guid bucketId, string parentPrefix, string suffix)
    {
        await _objects.Where(o => o.ParentPrefix == parentPrefix && o.BucketId == bucketId && o.Suffix == suffix).Delete().ExecuteAsync();
    }

    public async Task Delete(Guid bucketId, string parentPrefix)
    {
        await _objects.Where(o => o.ParentPrefix == parentPrefix && o.BucketId == bucketId).Delete().ExecuteAsync();
    }

    public async Task Create(ObjectSuffixByParentPrefix objectSuffixByParentPrefix)
    {
        await _objects.Insert(objectSuffixByParentPrefix).ExecuteAsync();
    }

    public async Task<ObjectSuffixByParentPrefix?> Any(Guid bucketId, string parentPrefix)
    {
        return await _objects.FirstOrDefault(o => o.ParentPrefix == parentPrefix && o.BucketId == bucketId).ExecuteAsync();
    }

    public async Task<ObjectSuffixesPage> GetPage(Guid bucketId, string parentPrefix, int pageSize,
        ObjectSuffixesPage.ObjectSuffixesPageCursor cursor)
    {
        const string queryString = """

                                                       SELECT * FROM object_suffixes_by_parent_prefix 
                                                       WHERE bucket_id = ? 
                                                       AND parent_prefix = ? 
                                                       AND (type, suffix) > (?, ?)
                                                       ORDER BY type , suffix
                                                       LIMIT ?
                                   """;

        var cql = new Cql(queryString, bucketId, parentPrefix, cursor.Type.ToString(), cursor.Suffix, pageSize + 1);

        var mapper = new Mapper(_objects.GetSession());

        var results = (await mapper.FetchAsync<ObjectSuffixByParentPrefix>(cql).ConfigureAwait(false)).ToList();

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
}
