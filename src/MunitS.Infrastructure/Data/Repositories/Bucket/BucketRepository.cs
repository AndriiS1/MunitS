using Cassandra;
using Microsoft.Extensions.Options;
using MunitS.Infrastructure.Options.DataBase;
namespace MunitS.Infrastructure.Data.Repositories.Bucket;

public class BucketRepository(ISession session, IOptions<DataBaseOptions> options): IBucketRepository
{
    private string Keyspace { get; } = options.Value.KeySpace;
    private string Table { get; } = options.Value.Tables.Buckets;
    public async Task<Domain.Bucket.Bucket?> Get(string name)
    {
        var getBucketStatement = await session.PrepareAsync($"SELECT * FROM {Keyspace}.{Table} WHERE {nameof(Domain.Bucket.Bucket.Name)} = ?");
        
        var statement = getBucketStatement.Bind(name);
        
        var row = (await session.ExecuteAsync(statement)).FirstOrDefault();

        return row == null ? null : Domain.Bucket.Bucket.FromDataInstance(row);
    }
}
