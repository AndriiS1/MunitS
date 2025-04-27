using Cassandra.Data.Linq;
using MunitS.Domain.Bucket.BucketCounter;
namespace MunitS.Infrastructure.Data.Repositories.Bucket.BucketCounter;

public class BucketCounterRepository(CassandraConnector connector) : IBucketCounterRepository
{
    private readonly Table<Domain.Bucket.BucketCounter.BucketCounter> _bucketCounters = new(connector.GetSession());

    public async Task<Domain.Bucket.BucketCounter.BucketCounter?> Get(Guid bucketId)
    {
        return (await _bucketCounters.Where(d => d.Id == bucketId).ExecuteAsync()).FirstOrDefault();
    }

    public async Task<List<Domain.Bucket.BucketCounter.BucketCounter>> GetAll(IEnumerable<Guid> bucketIds)
    {
        return (await _bucketCounters.Where(d => bucketIds.Contains(d.Id)).ExecuteAsync()).ToList();
    }

    public async Task IncrementObjectsCount(Guid bucketId, long increment = 1)
    {
        const string cql = $"UPDATE {BucketCountersMapping.TableName} "
                           + $"SET {BucketCountersMapping.ObjectsCountColumnName} = "
                           + $"{BucketCountersMapping.ObjectsCountColumnName} + ? WHERE id = ?";

        var prepared = await _bucketCounters.GetSession().PrepareAsync(cql);
        var bound = prepared.Bind(increment, bucketId);
        await _bucketCounters.GetSession().ExecuteAsync(bound).ConfigureAwait(false);
    }

    public async Task IncrementSizeInBytesCount(Guid bucketId, long increment)
    {
        const string cql = $"UPDATE {BucketCountersMapping.TableName}"
                           + $" SET {BucketCountersMapping.SizeInBytesColumnName} ="
                           + $" {BucketCountersMapping.SizeInBytesColumnName} + ? WHERE id = ?";

        var prepared = await _bucketCounters.GetSession().PrepareAsync(cql);
        var bound = prepared.Bind(increment, bucketId);
        await _bucketCounters.GetSession().ExecuteAsync(bound).ConfigureAwait(false);
    }

    public async Task Delete(Guid id)
    {
        await _bucketCounters.Where(b => b.Id == id).Delete().ExecuteAsync();
    }
}
