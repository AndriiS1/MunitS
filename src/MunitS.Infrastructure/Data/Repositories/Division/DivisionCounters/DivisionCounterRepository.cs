using Cassandra.Data.Linq;
using MunitS.Domain.Division.DivisionByBucketId;
using MunitS.Domain.Division.DivisionCounter;
namespace MunitS.Infrastructure.Data.Repositories.Division.DivisionCounters;

public class DivisionCounterRepository(CassandraConnector connector) : IDivisionCounterRepository
{
    private readonly Table<DivisionCounter> _divisions = new(connector.GetSession());

    public async Task<List<DivisionCounter>> GetAll(Guid bucketId, DivisionType.SizeType type)
    {
        return (await _divisions.Where(d => d.BucketId == bucketId && d.Type == type.ToString())
            .ExecuteAsync()).ToList();
    }

    public async Task IncrementObjectsCount(Guid bucketId, DivisionType.SizeType type, Guid id, long increment = 1)
    {
        const string cql = $"UPDATE {DivisionCountersMapping.TableName}" +
                           $" SET {DivisionCountersMapping.ObjectsCountColumnName} " +
                           $"= {DivisionCountersMapping.ObjectsCountColumnName} + ? WHERE bucket_id = ? AND type = ? AND id = ?";

        var prepared = await _divisions.GetSession().PrepareAsync(cql);
        var bound = prepared.Bind(increment, bucketId, type.ToString(), id);
        await _divisions.GetSession().ExecuteAsync(bound).ConfigureAwait(false);
    }
}
