using Cassandra;
using Microsoft.Extensions.Options;
using MunitS.Infrastructure.Options.DataBase;
namespace MunitS.Infrastructure.Data.Repositories.Object;

public class ObjectRepository(ISession session, IOptions<DataBaseOptions> options): IObjectRepository
{
    private string Keyspace { get; } = options.Value.KeySpace;
    private string Table { get; } = options.Value.Tables.Objects;
    public async Task<Domain.Object.Object?> Get(Guid bucketId, string fileKey)
    {
        var getObjectStatement = await session.PrepareAsync($"SELECT * FROM {Keyspace}.{Table} WHERE {nameof(Domain.Object.Object.BucketId)} = ? AND {nameof(Domain.Object.Object.FileKey)} = ?");
        
        var statement = getObjectStatement.Bind(bucketId, fileKey);
        
        var row = (await session.ExecuteAsync(statement)).FirstOrDefault();
        
        return row == null ? null : Domain.Object.Object.FromDataInstance(row);
    }
    
    public async Task Create(Domain.Object.Object @object)
    {
        var insertQuery = new SimpleStatement($"INSERT INTO {Keyspace}.{Table} (id, name, price) VALUES {@object.ToCreateInstance()}");
        
        await session.ExecuteAsync(insertQuery);
    }
}
