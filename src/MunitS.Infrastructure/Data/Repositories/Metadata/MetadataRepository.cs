using Cassandra;
using Microsoft.Extensions.Options;
using MunitS.Infrastructure.Options.DataBase;

namespace MunitS.Infrastructure.Data.Repositories.Metadata;

public class MetadataRepository(CassandraHelper cassandraHelper, IOptions<DataBaseOptions> options) : IMetadataRepository
{
    private readonly ISession _session = cassandraHelper.GetSession();
    private string Keyspace { get; } = options.Value.KeySpace;
    private string Table { get; } = options.Value.Tables.Metadata;
    
    public async Task Create(Domain.Metadata.Metadata metadata)
    {
        var query = $"INSERT INTO {Keyspace}.{Table} (id, name, price) VALUES ({Guid.NewGuid()}, 'First insert', '123')";
        await _session.ExecuteAsync(new SimpleStatement(query));
    }
    public Task<Domain.Metadata.Metadata?> Get(string fileKey)
    {
        throw new NotImplementedException();
    }
}
