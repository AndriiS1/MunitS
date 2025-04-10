using Cassandra.Data.Linq;
using MunitS.Domain.Division.DivisionByBucketId;
namespace MunitS.Infrastructure.Data.Repositories.Division;

public class DivisionRepository(CassandraConnector connector) : IDivisionRepository
{
    private readonly Table<DivisionByBucketId> _divisions = new (connector.GetSession());
    
    public async Task Create(DivisionByBucketId metadata)
    {
        await _divisions.Insert(metadata).ExecuteAsync();
    }
    
    public async Task<DivisionByBucketId?> GetNotFull(Guid bucketId, DivisionType divisionType)
    {
        return (await _divisions.Where(d => d.BucketId == bucketId 
                                              && d.Type == divisionType.Type.ToString() && d.ObjectsCount < divisionType.ObjectsCountLimit)
            .AllowFiltering().ExecuteAsync()).FirstOrDefault();
    }
    
    public async Task IncrementObjectsCount(Guid bucketId, DivisionType.SizeType type, Guid id, long increment = 1)
    {
        const string cql = $"UPDATE {DivisionsByBucketIdMapping.TableName} SET {DivisionsByBucketIdMapping.ObjectsCount} = {DivisionsByBucketIdMapping.ObjectsCount} + ? WHERE bucket_id = ? AND type = ? AND id = ?";

        var prepared = await _divisions.GetSession().PrepareAsync(cql);
        var bound = prepared.Bind(increment, bucketId, type.ToString(), id);
        await _divisions.GetSession().ExecuteAsync(bound).ConfigureAwait(false);
    }
}
