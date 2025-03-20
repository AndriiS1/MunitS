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
}
