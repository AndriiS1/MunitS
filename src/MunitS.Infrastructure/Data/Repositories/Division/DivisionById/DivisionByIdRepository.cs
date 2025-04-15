using Cassandra.Data.Linq;
using MunitS.Domain.Division.DivisionByBucketId;
namespace MunitS.Infrastructure.Data.Repositories.Division.DivisionById;

public class DivisionByIdRepository(CassandraConnector connector) : IDivisionByIdRepository
{
    private readonly Table<DivisionByBucketId> _divisions = new(connector.GetSession());

    public async Task Create(DivisionByBucketId metadata)
    {
        await _divisions.Insert(metadata).ExecuteAsync();
    }

    public async Task<List<DivisionByBucketId>> GetAll(Guid bucketId, DivisionType.SizeType type)
    {
        return (await _divisions.Where(d => d.BucketId == bucketId && d.Type == type.ToString())
            .ExecuteAsync()).ToList();
    }
}
