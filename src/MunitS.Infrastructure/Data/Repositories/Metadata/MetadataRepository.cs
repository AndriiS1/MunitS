using Cassandra;

namespace MunitS.Infrastructure.Data.Repositories.Metadata;

public class MetadataRepository(CassandraHelper cassandraHelper) : IMetadataRepository
{
    private readonly ISession _session = cassandraHelper.GetSession();
    
    public async Task Create(Domain.Metadata.Metadata metadata)
    {
        var query = $"INSERT INTO metadata (id, name, price) VALUES ({Guid.NewGuid()}, 'First insert', '123')";
        await _session.ExecuteAsync(new SimpleStatement(query));
    }
}
