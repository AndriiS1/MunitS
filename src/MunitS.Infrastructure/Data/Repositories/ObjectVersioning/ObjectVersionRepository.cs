using Cassandra;
using Microsoft.Extensions.Options;
using MunitS.Domain.Versioning;
using MunitS.Infrastructure.Options.DataBase;
namespace MunitS.Infrastructure.Data.Repositories.ObjectVersioning;

public class ObjectVersionRepository(ISession session, IOptions<DataBaseOptions> options): IObjectVersionRepository
{
    private string Keyspace { get; } = options.Value.KeySpace;
    private string Table { get; } = options.Value.Tables.Versions;
    
    public async Task Create(ObjectVersion version)
    {
        var insertQuery = new SimpleStatement($"INSERT INTO {Keyspace}.{Table} (id, name, price) VALUES {version.ToCreateInstance()}");
        
        await session.ExecuteAsync(insertQuery);
    }
    
    public async Task<List<ObjectVersion>> GetAll(Guid objectId)
    {
        var getObjectStatement = await session.PrepareAsync($"SELECT * FROM {Keyspace}.{Table} WHERE {nameof(ObjectVersion.ObjectId)} = ?");
        var statement = getObjectStatement.Bind(objectId);
        
        var rowSet = await session.ExecuteAsync(statement);
        
        return rowSet.Select(ObjectVersion.FromDataInstance).ToList();
    }
    
    private async Task<ObjectVersion?> GetOldestVersionAsync(Guid objectId)
    {
        var query = $"SELECT * FROM {Keyspace}.{Table} WHERE {nameof(ObjectVersion.ObjectId)} = ? ORDER BY uploaded_at ASC LIMIT 1";
        var statement = await session.PrepareAsync(query);
        var boundStatement = statement.Bind(objectId);
        
        var rowSet = await session.ExecuteAsync(boundStatement);
        var row = rowSet.FirstOrDefault();
        
        return row == null ? null : ObjectVersion.FromDataInstance(row);

    }
    
    public async Task DeleteOldest(Guid objectId)
    {
        var oldestVersion = await GetOldestVersionAsync(objectId);
        
        if (oldestVersion == null) return;
        
        var deleteStatement = await session.PrepareAsync($"DELETE FROM {Keyspace}.{Table} WHERE {nameof(ObjectVersion.ObjectId)} = ? AND {nameof(ObjectVersion.Id)} = ?");
        var statement = deleteStatement.Bind(objectId, oldestVersion.Id);
        
        await session.ExecuteAsync(statement);
    }
}
